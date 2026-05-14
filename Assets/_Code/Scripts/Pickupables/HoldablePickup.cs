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

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            FindParent();
        }

        private void FindParent()
        {
            if (!_rigidbody) return;
            if (_isHolding) return;
            if (transform.parent != null) return;
            if (_rigidbody.angularVelocity.magnitude < stopVelocityThreshold &&
                _rigidbody.angularVelocity.magnitude < stopVelocityThreshold)
            {
                Vector3 worldCenter = transform.TransformPoint(transform.localPosition);
                Quaternion worldRotation = transform.rotation;
                Collider[] hits = new Collider[25];
                Physics.OverlapBoxNonAlloc(worldCenter, Vector3.one, hits, worldRotation, layerMask);
                foreach (var hit in hits)
                {
                    var room = hit?.GetComponentInParent<Room>();
                    if (room)
                    {
                        transform.parent = room.transform;
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
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}