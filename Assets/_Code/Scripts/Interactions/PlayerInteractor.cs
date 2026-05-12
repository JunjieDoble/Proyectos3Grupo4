using _Code.Scripts.Character;
using _Code.Scripts.Pickupables;
using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Scripts.Interactions
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerInteractor : MonoBehaviour, IInteractor, IController
    {
        [Header("Raycast")]
        [SerializeField] private Transform viewOrigin;
        [SerializeField] private LayerMask interactableMask;
        
        [Header("Refs")]
        public Transform handTransform;

        private PlayerParameters _playerParameters;
        private IInteractable _currentInteractable;
        private HoldablePickup _currentPickupable;
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        public void LoadPlayerParameters(PlayerParameters playerParameters) => _playerParameters = playerParameters;
        
        public bool IsEnabled { get; set; }

        private void Awake()
        {
            if (viewOrigin == null)
            {
                Camera cam = Camera.main;
                viewOrigin = cam != null ? cam.transform : transform;
            }
            if (interactableMask.value == 0) Debug.LogWarning("Unassigned interactableMask in PlayerInteraction");
            IsEnabled = false;
            GetComponent<Player>()?.AddController(this);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!IsEnabled) return;
            if (context.started && TryRaycastInteract()) InteractionStarted();
            if (context.performed && _currentInteractable != null) InteractionPerformed();
            if (context.canceled && _currentInteractable != null) InteractionCanceled();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!IsEnabled) return;
            if (context.canceled && _currentPickupable != null)
            {
                _currentPickupable.Drop();
                _currentPickupable = null;
                return;
            }
            if (context.performed && _currentPickupable != null)
            {
                _currentPickupable.Throw(handTransform.forward * 10f);
                _currentPickupable = null;
            }
        }

        private bool TryRaycastInteract()
        {
            if (Physics.Raycast(viewOrigin.position, viewOrigin.forward, out var hit, _playerParameters.interactionDistance, interactableMask))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable == null) return false;
                _currentInteractable = interactable;
                return true;
            }
            return false;
        }

        private void InteractionStarted()
        {
            if (_currentInteractable is ILockable lockable && lockable.IsLocked()) return;
            if (_currentInteractable is HoldablePickup holdablePickup)
            {
                if (_currentPickupable != null) return;
                _currentPickupable = holdablePickup;
            } 
            _currentInteractable?.Interact(this);
        }

        private void InteractionPerformed()
        {
            if (_currentInteractable is IHoldInteractable holdInteractable)
            {
                holdInteractable.OnHoldCompleted(this);
            }
            _currentInteractable = null;
        }

        private void InteractionCanceled()
        {
            if (_currentInteractable is IHoldInteractable holdInteractable)
            {
                holdInteractable.OnHoldCanceled(this);
            }
            _currentInteractable = null;
        }
    }
}