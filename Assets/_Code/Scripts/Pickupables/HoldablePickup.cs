using _Code.Scripts.Interactions;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    [RequireComponent(typeof(Rigidbody))]
    public class HoldablePickup : PickupableBase
    {
        
        private Rigidbody _rigidbody;
        private bool _isHolding;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            Collider[] hits = Physics.OverlapSphere(point, 10f);
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
            Gizmos.DrawWireSphere(transform.position, 10f);
        }
    }
}