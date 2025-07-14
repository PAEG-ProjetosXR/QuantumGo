using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kynesis.Utilities;
using PokemonGO.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PokemonGO.Code
{
    public class Thrower : MonoBehaviour
    {
        [Header("Dragging Settings")]
        [SerializeField] private float _followSpeed = 10;
        [SerializeField] private float _torqueMultiplier = 0.3f;
        
        [Header("Throw Settings")]
        [SerializeField] private float _forceMultiplier = 6;
        [SerializeField] private float _heightMultiplier = 0.5f;
        [SerializeField] private float _curveInfluence = 5;
        [SerializeField] private float _minimumForce = 1f;
        [SerializeField, Range(-1f, 1f)] private float _minimumDot = 0.2f;
        
        [Header("Help Settings")]
        [SerializeField] private Vector3 _helpInfluence = new(0, 0, 0);
        [SerializeField] private float _helpRadius = 2;

        [Header("Bezier")]
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _mid;
        [SerializeField] private Transform _end;
        [SerializeField, Range(1, 10)] private float _extrapolation = 2;
        [SerializeField, Range(3, 100)] private int _points = 10;
        
        [Header("Bindings")]
        [SerializeField] private PokeBall _pokeBall;
        [SerializeField] private Transform _pokeBallSlot;
        [SerializeField] private Collider _pointerCollider;
        [SerializeField] private PokeBallFactory _pokeBallFactory;
        
        private Camera _mainCamera;
        private bool _isDragging;
        
        private float Force => Input.Instance.AveragePointerDelta.magnitude * _forceMultiplier;
        private bool HasPokeBall => _pokeBall != null;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            // O Input.Instance só deve ser acessado se ele já existir.
            if (Input.Instance != null)
            {
                Input.Instance.Pointer.started += OnPointerStarted;
                Input.Instance.Pointer.canceled += OnPointerCanceled;
            }
            
            // Cria a pokébola inicial.
            SpawnPokeBall();
        }

        private void OnDisable()
        {
            // É crucial verificar se o Input.Instance ainda existe antes de tentar se desinscrever.
            // Isso evita erros quando o jogo fecha.
            if (Input.Instance != null)
            {
                Input.Instance.Pointer.started -= OnPointerStarted;
                Input.Instance.Pointer.canceled -= OnPointerCanceled;
            }
        }

        private void Update()
        {
            if (!_isDragging) return;
            // FollowPointer();
        }

        private void FixedUpdate()
        {
            if (!_isDragging) return;
            AddToque();
        }

        private void StartDragging()
        {
            // _pokeBall.transform.position = GetPointerPosition();
            _pokeBall.transform.rotation = _pokeBallSlot.rotation;
            _pokeBall.DisableGravity();
            _pokeBall.ClearVelocities();
            _isDragging = true;
            StartCoroutine(DraggingEnumerator());
        }
        
        private void StopDragging()
        {
            _isDragging = false;
            float dot = Vector2.Dot(Input.Instance.AveragePointerDelta.normalized, Vector2.up);
            bool shouldThrow = dot > _minimumDot && Force > _minimumForce;
            if (shouldThrow)
                Throw();
            else
                Reset();
        }
        
        private void FollowPointer()
        {
            Vector3 pointerPosition = GetPointerPosition();
            Vector3 pokeBallPosition = _pokeBall.transform.position;
            _pokeBall.transform.position = Vector3.Slerp(pokeBallPosition, pointerPosition, Time.deltaTime * _followSpeed);
        }
        
        private void AddToque()
        {
            // Pega o quanto o mouse se moveu desde o último frame (um vetor 2D)
            Vector2 pointerDelta = Input.Instance.PointerDelta;

            // Nós queremos que o movimento horizontal do mouse (pointerDelta.x) gere um giro
            // em torno do eixo Y da bola (um giro para os lados, ou "sidespin").
            // O sinal negativo em -pointerDelta.x é para que o giro pareça mais natural.
            Vector3 torque = new Vector3(pointerDelta.y, -pointerDelta.x, 0) * _torqueMultiplier;

            // O script PokeBall irá receber este torque e aplicar à sua física.
            _pokeBall.AddTorque(torque);
        }
        private void Throw()
        {   
            Vector3 startPosition = _pokeBall.transform.position;
            _start.position = startPosition;
            Vector2 pointerInfluence = new Vector2(1f, 0f); 
            Vector3 pointerDirection = Input.Instance.AveragePointerDelta.normalized;
            Vector3 influencedPointerDirection = Vector3.Scale(pointerDirection, pointerInfluence);
            Vector3 localPointerDirection = transform.TransformDirection(influencedPointerDirection);
            Vector3 throwVector = (localPointerDirection + transform.forward).normalized * Force;
            Vector3 endPosition = startPosition + throwVector;
            Vector3 midPosition = Vector3.Lerp(startPosition, endPosition, 0.5f);
            midPosition.y += Force * _heightMultiplier * (_pokeBall.IsCharged ? 0.5f : 1);
            _mid.position = midPosition;
            
            if (_pokeBall.IsCharged)
            {
                Vector3 curveDirection = new Vector3(-localPointerDirection.x, 0, 0).normalized;
                endPosition += curveDirection * _curveInfluence;
            }

            Transform pokemon = GameObject.FindWithTag("Physicist").transform; // Usando a sua tag
            bool isOnHelpRange = Vector3.Distance(pokemon.position, endPosition) < _helpRadius;
            if(isOnHelpRange)
            {
                endPosition.x = Mathf.Lerp(endPosition.x, pokemon.position.x, _helpInfluence.x);
                endPosition.y = Mathf.Lerp(endPosition.y, pokemon.position.y, _helpInfluence.y);
                endPosition.z = Mathf.Lerp(endPosition.z, pokemon.position.z, _helpInfluence.z);
                _end.position = endPosition;
            }
            
            List<Vector3> path = Bezier.GetExtrapolatedPath(startPosition, midPosition, endPosition, 0f, _extrapolation, _points);
            
            _pokeBall.Throw(path);
            _pokeBall.transform.SetParent(null);
            _pokeBall = null;
            DOVirtual.DelayedCall(1, SpawnPokeBall);
        }
        
        public void SpawnPokeBall()
        {
            _pokeBall = _pokeBallFactory.Create(_pokeBallSlot.position, _pokeBallSlot.rotation);
            _pokeBall.transform.SetParent(_pokeBallSlot);
        }

        private void Reset()
        {   
            _pokeBall.ClearVelocities();
            _pokeBall.transform.position = _pokeBallSlot.position;
            _pokeBall.transform.rotation = _pokeBallSlot.rotation;
        }
        
        private Vector3 GetPointerPosition()
        {
            Vector2 pointerPosition = Input.Instance.PointerPosition;
            Ray ray = _mainCamera.ScreenPointToRay(pointerPosition);
            Physics.Raycast(ray, out RaycastHit grab, float.MaxValue, Layer.Mask.Pointer);
            return grab.point;
        }
        
        private bool IsOnPointerCollider()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.Instance.PointerPosition);
            Physics.Raycast(ray, out RaycastHit pointerHit, float.MaxValue, Layer.Mask.Pointer);
            return pointerHit.collider != null && pointerHit.collider == _pointerCollider;
        }
        
        private void OnPointerStarted(InputAction.CallbackContext callbackContext)
        {
            if (!IsOnPointerCollider() || !HasPokeBall) return;
            StartDragging();
        }

        private void OnPointerCanceled(InputAction.CallbackContext callbackContext)
        {
            if (!_isDragging) return;
            StopDragging();
        }
        
        private IEnumerator DraggingEnumerator()
        {   
            while (_isDragging)
            {
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (_start == null || _mid == null || _end == null) return;
            List<Vector3> path = Bezier.GetPath(_start.position, _mid.position, _end.position, _points);
            foreach (Vector3 point in path)
                Gizmos.DrawSphere(point, 0.05f);
        }
    }
}