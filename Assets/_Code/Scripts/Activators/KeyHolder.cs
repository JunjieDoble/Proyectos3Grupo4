using _Code.Scripts.Bases;
using _Code.Scripts.Interactions;
using _Code.Scripts.Pickupables;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Activators
{
    public class KeyHolder : Activator, IInteractable, IInteractor
    {
        [Header("Key Holder")] 
        [SerializeField] private Transform socket;
        [SerializeField] private HoldableType acceptedType = HoldableType.Key;

        private HoldablePickup _heldKey;

        public Transform Transform => socket;
        public GameObject GameObject => gameObject;

        public void Interact(IInteractor interactor)
        {
            if (interactor is not PlayerInteractor playerInteractor) return;
            if (_heldKey)
            {
                if (playerInteractor.CurrentPickupable) return;
                _heldKey.Drop();
                _heldKey.Interact(playerInteractor);
                playerInteractor.CurrentPickupable = _heldKey;
                _heldKey = null;
            }
            else
            {
                if (playerInteractor.CurrentPickupable == null || playerInteractor.CurrentPickupable.HoldableType != acceptedType) return;
                playerInteractor.CurrentPickupable.Drop();
                playerInteractor.CurrentPickupable.Interact(this);
                _heldKey = playerInteractor.CurrentPickupable;                
                playerInteractor.CurrentPickupable = null;
            }
            SetActive(_heldKey != null);
        }
    }
}