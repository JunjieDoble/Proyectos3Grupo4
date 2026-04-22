using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    public abstract class PickupableBase : MonoBehaviour, IInteractable
    {
        public virtual void Interact()
        {
            PickUp();
        }
        
        public abstract void PickUp();
    }
}
