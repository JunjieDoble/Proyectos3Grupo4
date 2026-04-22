using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    public class HoldablePickup : PickupableBase
    {
        public override void PickUp(IInteractor interactor)
        {
            if (interactor is PlayerInteractor player)
            {
                var rb = GetComponent<Rigidbody>();
                
                rb.isKinematic = true;
                rb.useGravity = false;
                
                transform.SetParent(player.handTransform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}