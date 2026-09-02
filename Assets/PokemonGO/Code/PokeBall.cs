using DG.Tweening;
using Kynesis.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace PokemonGO.Code
{
    // Enum adicionado para compatibilidade com a Factory
    public enum PokeBallType
    {
        Pokeball,
        Greatball,
        Ultraball,
        Masterball
    }

    public class PokeBall : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _chargedAngularSpeedPercentage;
        [SerializeField] private float _bounceMultiplier = 2;
        // SERÁ REMOVIDO
        //[SerializeField] private AnimationCurve _speedCurve;
        [SerializeField]
        private float _gravityMultiplier = 0.2f;

        [Header("Bindings")]
        [SerializeField] private Rigidbody _rigidbody;

        [NonSerialized]
        public int healthDamage;

        private bool _isCharged;
        private Vector3 _lastFramePosition;
        // SERÁ REMOVIDO
       // private Tween _followPathTween;

        // Eventos que o PokeBallVisual precisa
        public event Action OnCharged;
        public event Action OnDischarged;
        public event Action OnThrown;
        public event Action<Collision> OnCollision;

        public bool IsCharged => _isCharged;
        //private bool IsFollowingPath => _followPathTween is { active: true } && !_followPathTween.IsComplete();
        public Vector3 AngularVelocity => _rigidbody.angularVelocity;

        public EncounterManager encounterManager;
        public Atomball atomballInfo;

        private bool _isThrown = false;
        private bool targetDied;
        
        // ==========================================================
        //  MÉTODO ADICIONADO PARA CORRIGIR O ERRO DE REFERÊNCIA
        // ==========================================================
        private void Awake()
        {
            // Esta linha força o script a encontrar o componente Rigidbody
            // no mesmo GameObject em que ele está, resolvendo o erro.
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            encounterManager = FindAnyObjectByType<EncounterManager>();
        }
        // ==========================================================

        private void Update()
        {
            float chargedAngularSpeed = Physics.defaultMaxAngularSpeed * _chargedAngularSpeedPercentage;
            bool shouldCharge = Mathf.Abs(_rigidbody.angularVelocity.magnitude) > chargedAngularSpeed;

            if (shouldCharge && !_isCharged)
                Charge();

            if (!shouldCharge && _isCharged)
                Discharge();
        }

        private void FixedUpdate()
        {
           // _lastFramePosition = _rigidbody.position;

           if (!_isThrown)
                return;

            _rigidbody.AddForce(
                Physics.gravity * _gravityMultiplier,
                ForceMode.Acceleration
            );
        }

        bool hasCollided = false; // Variável para garantir que a colisão seja processada apenas uma vez
        private void OnCollisionEnter(Collision other)
        {
            
            if (hasCollided)
                return;

            if (other.gameObject.CompareTag("Physicist"))
            {
                hasCollided = true;
                PhysicistTrigger physicistTrigger =
                    other.gameObject.GetComponent<PhysicistTrigger>();
                if (physicistTrigger != null)
                {
                    PhysicistData physicistData = physicistTrigger.data;
                    // Procura as informações de captura correspondentes
                    for (int i = 0; i < physicistData.physicistCaptureInfo.Count; i++)
                    {
                        var capInfo = physicistData.physicistCaptureInfo[i];
                        if (capInfo.model == other.gameObject)
                        {
                            Debug.Log("Colidiu com Physicist!");
                            break;
                        }
                    }

                    // Reduz a vida do Physicist
                    targetDied = physicistTrigger.ReduceHp(healthDamage);

                    // Se a vida chegou a zero, inicia o encontro/captura
                    if (targetDied)
                    {
                        physicistTrigger.TriggerEncounter(atomballInfo.captureTimes);
                    }
                }

                // Destrói o Physicist se ele morreu
                if (targetDied)
                {
                    Destroy(other.gameObject);
                }

                // Destrói a Atomball depois de 2 segundos
                Destroy(gameObject, 2f);

                return;
            }

            if (other.gameObject.CompareTag("Object"))
            {
                hasCollided = true;

                ObjectTrigger objectTrigger =
                    other.gameObject.GetComponent<ObjectTrigger>();

                if (objectTrigger != null)
                {
                    ObjectData objectData = objectTrigger.data;
                    // Procura as informações de captura correspondentes
                    for (int i = 0; i < objectData.objectCaptureInfo.Count; i++)
                    {
                        var capInfo = objectData.objectCaptureInfo[i];

                        if (capInfo.model == other.gameObject)
                        {
                            capInfo.captureTime = DateTime.Now;

                            ARTrackedImage trackedImage =
                                capInfo.trackedImage;

                            capInfo.recaptureTime =
                                DateTime.Now.AddSeconds(objectData.waitRecaptureSecs);
                            break;
                        }
                    }

                    // Inicia o encontro
                    objectTrigger.TriggerEncounter(
                        atomballInfo.captureTimes
                    );
                }

                // Destrói a Atomball depois de 2 segundos
                Destroy(gameObject, 2f);

                return;
            }

            // A física do Rigidbody agora cuida do movimento.
            // SimulateBounce(other.GetContact(0)); // LEGADO


            OnCollision?.Invoke(other);
        }

        private void Charge()
        {
            _isCharged = true;
            OnCharged?.Invoke();
        }

        private void Discharge()
        {
            _isCharged = false;
            OnDischarged?.Invoke();
        }

        public void ClearVelocities()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void EnableGravity()
        {
            _rigidbody.useGravity = true;
        }

        public void DisableGravity()
        {
            _rigidbody.useGravity = false;
        }

        public void AddTorque(Vector3 torque)
        {
            _rigidbody.AddTorque(torque);
        }

        // novo throw
        public void Throw(Vector3 force)
        {
            _rigidbody.isKinematic = false;
            // só ao arremessar ativa gravidade!
            _isThrown = true;

            //ignora velocidade do "arrasto", tudo sera pela force do arremesso final
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _rigidbody.AddForce(force, ForceMode.Impulse);

            OnThrown?.Invoke();
        }

        
        //private void OnCompletePath()
        //{
        //    EnableGravity();
        //    Vector3 lastMotion = _rigidbody.position - _lastFramePosition;
        //    _rigidbody.AddForce(lastMotion, ForceMode.Impulse);
        //}

        private void SimulateBounce(ContactPoint contact)
        {
            Vector3 impactDirection = (_rigidbody.position - _lastFramePosition).normalized;
            Vector3 impactVelocity = impactDirection * _bounceMultiplier;
            Vector3 normal = contact.normal;
            Vector3 force = Vector3.Reflect(impactVelocity, normal);
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}