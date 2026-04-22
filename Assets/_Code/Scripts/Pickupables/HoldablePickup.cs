using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    [RequireComponent(typeof(Rigidbody))]
    public class HoldablePickup : PickupableBase
    {
        
        private Rigidbody _rigidbody;

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
            }
        }
        
        public void Drop()
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
        }

        public void Throw(Vector3 force)
        {
            Drop();
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}