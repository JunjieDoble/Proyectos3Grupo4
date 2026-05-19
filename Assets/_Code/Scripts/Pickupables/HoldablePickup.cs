using _Code.Scripts.CheckPoint;
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
        [SerializeField] private float stopVelocityThreshold = 0.1f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float alertRadius = 10f;
        private Rigidbody _rigidbody;
        private bool _isHolding;
        private Vector3 _originalPosition;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _originalPosition = transform.position;
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
                Debug.Log("Finding parent for " + name + " angular velocity: " + _rigidbody.angularVelocity.magnitude + " velocity: " + _rigidbody.linearVelocity.magnitude);
                Collider[] hits = new Collider[25];
                Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * 0.5f, hits, transform.rotation, layerMask);
                foreach (var hit in hits)
                {
                    var room = hit?.GetComponent<Room>() ?? hit?.GetComponentInParent<Room>();
                    if (room)
                    {
                        transform.SetParent(room.transform);
                        Debug.Log(transform.parent + " holding " + name);
                        break;
                    }
                }
            }
        }
        
        public override void PickUp(IInteractor interactor)
        {
            if (interactor is PlayerInteractor player)
            {
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
                transform.SetParent(player.handTransform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                _isHolding = true;
            }
        }
        
        public void Drop()
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _isHolding = false;
        }

        public void Throw(Vector3 force)
        {
            Drop();
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (_rigidbody.linearVelocity.magnitude > 0.1f && !_isHolding)
            {
                Vector3 avgPoint = Vector3.zero;
                foreach (var contact in collision.contacts)
                    avgPoint += contact.point;
                avgPoint /= collision.contacts.Length;
                AlertNearbyEnemies(avgPoint);
            }
        }
        
        private void AlertNearbyEnemies(Vector3 point)
        {
            Collider[] hits = Physics.OverlapSphere(point, alertRadius);
            foreach (Collider hit in hits)
            {
                IEnemy enemy = hit.GetComponentInParent<IEnemy>();
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
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }

        public void Reset()
        {
            Drop();
            transform.position = _originalPosition;
        }
    }
}