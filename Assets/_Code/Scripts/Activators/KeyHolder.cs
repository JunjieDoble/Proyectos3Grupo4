using _Code.Scripts.Bases;
using _Code.Scripts.Gameplay;
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
        [SerializeField]
        private FMODUnity.EventReference activationSound;
        [SerializeField]
        private FMODUnity.EventReference deactivationSound;

        private HoldablePickup _heldKey;

        public Transform Transform => socket;
        public GameObject GameObject => gameObject;

        private void OnEnable()
        {
            GameManager.OnPlayerRespawn += Reset;
        }
        
        private void OnDisable()
        {
            GameManager.OnPlayerRespawn -= Reset;
        }
        
        private void Reset()
        {
            _heldKey?.Reset();
            _heldKey = null;
            SetActive(false);
        }
        
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
                if (!deactivationSound.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(deactivationSound, transform.position);
                }
            }
            else
            {
                if (playerInteractor.CurrentPickupable == null || playerInteractor.CurrentPickupable.HoldableType != acceptedType) return;
                playerInteractor.CurrentPickupable.Drop();
                playerInteractor.CurrentPickupable.Interact(this);
                _heldKey = playerInteractor.CurrentPickupable;                
                playerInteractor.CurrentPickupable = null;
                if (!activationSound.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(activationSound, transform.position);
                }
            }
            SetActive(_heldKey != null);
        }
    }
}