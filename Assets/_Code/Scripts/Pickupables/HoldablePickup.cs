using _Code.Scripts.CheckPoint;
using _Code.Scripts.Enemy;
using _Code.Scripts.Gameplay;
using _Code.Scripts.Interactions;
using _Code.Scripts.Rooms;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    [RequireComponent(typeof(Rigidbody))]
    public class HoldablePickup : PickupableBase
    {
        [Header("Settings")]
        [SerializeField] private HoldableType holdableType = HoldableType.Distraction;
        [SerializeField] private float stopVelocityThreshold = 0.1f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float alertRadius = 10f;
        [SerializeField] private Vector3 parentSearchScale;
        private Rigidbody _rigidbody;
        private bool _isHolding;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private int _originalLayerMask;

        public HoldableType HoldableType => holdableType;
        
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
            _originalLayerMask = gameObject.layer;
        }

        void OnEnable()
        {
            GameManager.OnPlayerRespawn += Reset;
            Checkpoint.OnCheckpointChange += UpdateOrigin;
        }
        
        void OnDisable()
        {
            GameManager.OnPlayerRespawn -= Reset;
            Checkpoint.OnCheckpointChange -= UpdateOrigin;
        }
        
        void UpdateOrigin()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        void FixedUpdate()
        {
            FindParent();
        }

        private void FindParent()
        {
            if (!_rigidbody) return;
            if (_isHolding) return;
            if (transform.parent) return;
            if (_rigidbody.angularVelocity.magnitude < stopVelocityThreshold &&
                _rigidbody.linearVelocity.magnitude < stopVelocityThreshold)
            {
                Collider[] hits = new Collider[25];
                Physics.OverlapBoxNonAlloc(transform.position, parentSearchScale/2, hits, transform.rotation, layerMask);
                foreach (var hit in hits)
                {
                    var room = hit?.GetComponent<Room>() ?? hit?.GetComponentInParent<Room>();
                    if (room)
                    {
                        transform.SetParent(room.transform);
                        _rigidbody.isKinematic = true;
                        break;
                    }
                }
            }
        }
        
        public override void PickUp(IInteractor interactor)
        {
            if  (_isHolding) return;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            transform.SetParent(interactor.Transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _isHolding = true;
            gameObject.layer = interactor.Transform.gameObject.layer;
        }
        
        public void Drop()
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _isHolding = false;
            gameObject.layer = _originalLayerMask;
        }

        public void Throw(Vector3 force)
        {
            Drop();
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        void OnCollisionEnter()
        {
            if (_rigidbody.linearVelocity.magnitude > 0.1f && !_isHolding)
            {
                AlertNearbyEnemies(transform.position);
            }
        }
        
        private void AlertNearbyEnemies(Vector3 point)
        {
            var results = new Collider[25];
            Physics.OverlapSphereNonAlloc(point, alertRadius, results);
            foreach (Collider hit in results)
            {
                EnemyBehaviour enemy = hit?.GetComponentInParent<EnemyBehaviour>();
                if (enemy != null)
                {
                    enemy.AlertEnemy(point);
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, alertRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, parentSearchScale);
        }

        public void Reset()
        {
            Drop();
            transform.position = _originalPosition;
            transform.rotation = _originalRotation;
        }
    }
}