using _Code.Scripts.Interactions;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    public abstract class PickupableBase : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private FMODUnity.EventReference pickupSound;
        public GameObject GameObject => gameObject;
        public virtual void Interact(IInteractor interactor)
        {
            PickUp(interactor);
            if (!pickupSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(pickupSound, transform.position);
            }
        }
        
        public abstract void PickUp(IInteractor interactor);
    }
}
