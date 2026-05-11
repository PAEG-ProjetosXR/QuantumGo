using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kynesis.Utilities;
using PokemonGO.Global;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

namespace PokemonGO.Code
{
    public class Thrower : MonoBehaviour
    {
        [Header("Dragging Settings")]
        [SerializeField, Range(0f, 1f)]
        private float _verticalInfluence = 0.5f;

        [SerializeField]
        private LayerMask _pointerLayerMask;

        [SerializeField]
        private float _followSpeed = 10;

        [SerializeField]
        private float _torqueMultiplier = 0.3f;

        [SerializeField]
        private Vector3 _heldPokeBallScale = Vector3.one;

        [Header("Throw Settings")]
        [SerializeField]
        private float _forceMultiplier = 6;

        [SerializeField]
        private float _heightMultiplier = 0.5f;

        [SerializeField]
        private float _curveInfluence = 5;

        [SerializeField]
        private float _minimumForce = 1f;

        [SerializeField, Range(-1f, 1f)]
        private float _minimumDot = 0.2f;

        [Header("Help Settings")]
        [SerializeField]
        private Vector3 _helpInfluence = new(0, 0, 0);

        [SerializeField]
        private float _helpRadius = 2;

        [Header("Bezier")]
        [SerializeField]
        private Transform _start;

        [SerializeField]
        private Transform _mid;

        [SerializeField]
        private Transform _end;

        [SerializeField, Range(1, 10)]
        private float _extrapolation = 2;

        [SerializeField, Range(3, 100)]
        private int _points = 10;

        [Header("Bindings")]
        [SerializeField]
        private PokeBall _pokeBall;

        [SerializeField]
        private Transform _pokeBallSlot;

        [SerializeField]
        private Collider _pointerCollider;

        [SerializeField]
        private PokeBallFactory _pokeBallFactory;

        private ParticleSystem _dragParticles;

        private Camera _mainCamera;

        private bool _isDragging;
        private Transform _pokemon;

        private Vector2 _touchStartPosition;

        private Vector2 _touchEndPosition;

        private Vector2 _currentPointerDelta;

        private float Force =>
            _currentPointerDelta.magnitude * _forceMultiplier;

        private bool HasPokeBall =>
            _pokeBall != null && _pokeBall.gameObject != null;

        public static Thrower Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (_pokeBallSlot == null &&
                CameraManager.Instance != null)
            {
                _pokeBallSlot =
                    CameraManager.Instance.PokeBallHoldPosition;
            }

            Debug.Log(
                "Thrower ativo em: " +
                gameObject.name +
                " | Cena: " +
                gameObject.scene.name
            );
        }

        private void Start()
        {
            var xr = FindAnyObjectByType<XROrigin>();

            if (xr != null)
            {
                _mainCamera =
                    xr.GetComponentInChildren<Camera>();
            }

            if (_mainCamera == null)
            {
                Debug.LogError("Main Camera não encontrada!");
            }

            if (_start == null ||
                _mid == null ||
                _end == null)
            {
                Debug.LogError("Bezier points não definidos!");
            }
        }

        private void Update()
        {
            // Spawn no PC
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                if (!HasPokeBall)
                {
                    SpawnPokeBall();
                }
            }

            // MOBILE
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch =
                    UnityEngine.Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        Debug.Log("TOQUE COMEÇOU");

                        if (HasPokeBall)
                        {
                            _touchStartPosition =
                                touch.position;

                            StartDragging();
                        }

                        break;

                    case TouchPhase.Moved:

                        if (_isDragging)
                        {
                            _currentPointerDelta =
                                touch.position -
                                _touchStartPosition;

                            FollowPointer();
                        }

                        break;

                    case TouchPhase.Ended:

                        Debug.Log("TOQUE TERMINOU");

                        if (_isDragging)
                        {
                            _touchEndPosition =
                                touch.position;

                            _currentPointerDelta =
                                _touchEndPosition -
                                _touchStartPosition;

                            StopDragging();
                        }

                        break;
                }
            }

            // PC
            else
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    if (HasPokeBall)
                    {
                        _touchStartPosition =
                            UnityEngine.Input.mousePosition;

                        StartDragging();
                    }
                }

                if (UnityEngine.Input.GetMouseButton(0))
                {
                    if (_isDragging)
                    {
                        _currentPointerDelta =
                            (Vector2)UnityEngine.Input.mousePosition -
                            _touchStartPosition;

                        FollowPointer();
                    }
                }

                if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    if (_isDragging)
                    {
                        _touchEndPosition =
                            UnityEngine.Input.mousePosition;

                        _currentPointerDelta =
                            _touchEndPosition -
                            _touchStartPosition;

                        StopDragging();
                    }
                }
            }
        }

        private IEnumerator DraggingEnumerator()
        {
            while (_isDragging)
            {
                yield return null;
            }
        }

        private void StartDragging()
        {
            if (!HasPokeBall)
                return;

            _pokeBall.transform.rotation =
                _pokeBallSlot.rotation;

            _pokeBall.DisableGravity();

            _pokeBall.ClearVelocities();

            _isDragging = true;

            if (_dragParticles != null)
            {
                _dragParticles.Play();
            }

            StartCoroutine(DraggingEnumerator());
        }

        private void StopDragging()
        {
            if (Input.Instance == null)
                return;
            _isDragging = false;

            if (_dragParticles != null)
            {
                _dragParticles.Stop();
            }

            float dot =
                Vector2.Dot(
                    _currentPointerDelta.normalized,
                    Vector2.up
                );

            bool shouldThrow =
                dot > _minimumDot &&
                Force > _minimumForce;

            if (shouldThrow)
            {
                Throw();
            }
            else
            {
                Reset();
            }
        }

        private void FollowPointer()
        {
            if (!HasPokeBall || _mainCamera == null)
                return;

            Vector3 screenPosition;

            // MOBILE
            if (UnityEngine.Input.touchCount > 0)
            {
                screenPosition =
                    UnityEngine.Input.GetTouch(0).position;
            }
            // PC
            else
            {
                screenPosition =
                    UnityEngine.Input.mousePosition;
            }

            screenPosition.z = 1.0f;

            Vector3 targetPosition =
                _mainCamera.ScreenToWorldPoint(
                    screenPosition
                );

            _pokeBall.transform.position =
                Vector3.Lerp(
                    _pokeBall.transform.position,
                    targetPosition,
                    Time.deltaTime * _followSpeed
                );

            float rotationSpeed =
                _currentPointerDelta.magnitude * 10f;

            _pokeBall.transform.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.World
            );
        }

        private void FixedUpdate()
        {
            if (!_isDragging)
                return;

            AddTorque();
        }

        private void AddTorque()
        {
            if (!HasPokeBall)
                return;

            Vector2 pointerDelta =
                _currentPointerDelta;

            Vector3 torque =
                new Vector3(
                    pointerDelta.y,
                    -pointerDelta.x,
                    0
                ) * _torqueMultiplier;

            _pokeBall.AddTorque(torque);
        }

        private void Throw()
        {
            if (!HasPokeBall || _mainCamera == null)
                return;

            GameObject pokemonObj =
                GameObject.FindWithTag("Physicist");

            if (pokemonObj == null)
            {
                Debug.LogWarning(
                    "Physicist não encontrado!"
                );
                return;
            }

            Transform pokemon =
                pokemonObj.transform;

            Vector3 startPosition =
                _pokeBall.transform.position;

            _start.position = startPosition;

            Vector2 pointerInfluence =
                new Vector2(1f, 1f);

            Vector3 pointerDirection =
                _currentPointerDelta.normalized;

            Vector3 influencedPointerDirection =
                Vector3.Scale(
                    pointerDirection,
                    pointerInfluence
                );

            Vector3 swipeDirection =
                influencedPointerDirection;

            Vector3 horizontalThrow =
                _mainCamera.transform.TransformDirection(
                    new Vector3(
                        swipeDirection.x,
                        0,
                        0
                    )
                );

            Vector3 verticalThrow =
                Vector3.up *
                swipeDirection.y *
                _verticalInfluence;

            Vector3 throwVector =
                (
                    _mainCamera.transform.forward +
                    horizontalThrow +
                    verticalThrow
                ).normalized * Force;

            Vector3 endPosition =
                startPosition + throwVector;

            Vector3 midPosition =
                Vector3.Lerp(
                    startPosition,
                    endPosition,
                    0.5f
                );

            midPosition.y +=
                Force *
                _heightMultiplier *
                (_pokeBall.IsCharged ? 0.5f : 1);

            _mid.position = midPosition;

            if (_pokeBall.IsCharged)
            {
                Vector3 curveDirection =
                    new Vector3(
                        -horizontalThrow.x,
                        0,
                        0
                    ).normalized;

                endPosition +=
                    curveDirection *
                    _curveInfluence;
            }

            bool isOnHelpRange =
                Vector3.Distance(
                    pokemon.position,
                    endPosition
                ) < _helpRadius;

            if (isOnHelpRange)
            {
                endPosition.x =
                    Mathf.Lerp(
                        endPosition.x,
                        pokemon.position.x,
                        _helpInfluence.x
                    );

                endPosition.y =
                    Mathf.Lerp(
                        endPosition.y,
                        pokemon.position.y,
                        _helpInfluence.y
                    );

                endPosition.z =
                    Mathf.Lerp(
                        endPosition.z,
                        pokemon.position.z,
                        _helpInfluence.z
                    );

                _end.position = endPosition;
            }

            List<Vector3> path =
                Bezier.GetExtrapolatedPath(
                    startPosition,
                    midPosition,
                    endPosition,
                    0f,
                    _extrapolation,
                    _points
                );

            _pokeBall.Throw(path);

            _pokeBall.transform.SetParent(null);

            _pokeBall = null;
        }

        public void SpawnPokeBall()
        {
            if (HasPokeBall)
            {
                Debug.Log(
                    "já existe uma pokebola ativa!"
                );
                return;
            }

            if (_mainCamera == null)
            {
                Debug.LogError(
                    "AR Camera is not assigned!"
                );
                return;
            }

            _pokeBall =
                _pokeBallFactory.Create(
                    Vector3.zero,
                    Quaternion.identity
                );

            if (_pokeBall == null)
            {
                Debug.LogError(
                    "Model to Place is not assigned!"
                );
                return;
            }

            Transform cam =
                _mainCamera.transform;

            _pokeBall.transform.SetParent(cam);

            _pokeBall.transform.localPosition =
                new Vector3(0, -0.4f, 1f);

            _pokeBall.transform.localRotation =
                Quaternion.identity;

            _pokeBall.transform.localScale =
                _heldPokeBallScale;

            Transform particleTransform =
                _pokeBall.transform.Find(
                    "Visual/VFX/RotationVFX"
                );

            if (particleTransform != null)
            {
                _dragParticles =
                    particleTransform.GetComponent<ParticleSystem>();
            }
            else
            {
                Debug.LogWarning(
                    "RotationVFX não encontrado!"
                );
            }
        }

        private void Reset()
        {
            if (!HasPokeBall)
                return;

            _pokeBall.ClearVelocities();

            _pokeBall.transform.position =
                _pokeBallSlot.position;

            _pokeBall.transform.rotation =
                _pokeBallSlot.rotation;
        }

        private Vector3 GetPointerPosition()
        {
            if (_mainCamera == null)
                return Vector3.zero;

            Vector2 pointerPosition;

            if (UnityEngine.Input.touchCount > 0)
            {
                pointerPosition =
                    UnityEngine.Input.GetTouch(0).position;
            }
            else
            {
                pointerPosition =
                    UnityEngine.Input.mousePosition;
            }

            Ray ray =
                _mainCamera.ScreenPointToRay(
                    pointerPosition
                );

            if (
                Physics.Raycast(
                    ray,
                    out RaycastHit grab,
                    float.MaxValue
                )
            )
            {
                return grab.point;
            }

            return _pokeBallSlot != null
                ? _pokeBallSlot.position
                : Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            if (_start == null ||
                _mid == null ||
                _end == null)
            {
                return;
            }

            List<Vector3> path =
                Bezier.GetPath(
                    _start.position,
                    _mid.position,
                    _end.position,
                    _points
                );

            foreach (Vector3 point in path)
            {
                Gizmos.DrawSphere(point, 0.05f);
            }
        }

        private void LateUpdate()
        {
            if (CameraManager.Instance == null)
            {
                return;
            }

            Transform anchor =
                CameraManager.Instance.PlayerInputAnchor;
        }
    }
}