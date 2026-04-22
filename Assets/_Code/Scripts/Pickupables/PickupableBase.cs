using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    public abstract class PickupableBase : MonoBehaviour, IInteractable
    {
        public virtual void Interact(IInteractor interactor)
        {
            PickUp(interactor);
        }
        
        public abstract void PickUp(IInteractor interactor);
    }
}
