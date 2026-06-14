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
        
        public HoldablePickup CurrentPickupable
        {
            get => _currentPickupable;
            set => _currentPickupable = value;
        }
        
        public Transform Transform => handTransform;
        public GameObject GameObject => gameObject;

        public void LoadPlayerParameters(PlayerParameters playerParameters) => _playerParameters = playerParameters;
        public void OnPlayerRespawn(Vector3 _)
        {
            _currentPickupable?.Reset();
            ClearInteractable();
            _currentPickupable = null;
        }
        
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

        void FixedUpdate()
        {
            if (!IsEnabled) return;
            TryRaycastInteract();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!IsEnabled) return;
            if (context.started && _currentInteractable != null) InteractionStarted();
            if (context.performed && _currentInteractable != null) InteractionPerformed();
            if (context.canceled && _currentInteractable != null) InteractionCanceled();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!IsEnabled) return;
            if (context.canceled && _currentPickupable != null)
            {
                if (!CheckValidDrop()) return;
                _currentPickupable.Drop();
                _currentPickupable = null;
                return;
            }
            if (context.performed && _currentPickupable != null)
            {
                if (!CheckValidDrop()) return;
                _currentPickupable.Throw(viewOrigin.forward * 10f);
                _currentPickupable = null;
            }
        }

        private bool CheckValidDrop()
        {
            if (_currentPickupable == null) return false;
            Ray ray = new Ray(viewOrigin.position, viewOrigin.forward);
            if (Physics.Raycast(ray, out var hit, _playerParameters.throwCheckDistance, interactableMask))
            {
                if (hit.collider.GetComponentInParent<IInteractable>() != null)
                {
                    Debug.Log("Cannot throw, object in the way");
                    return false;
                }
            }
            return true;
        }

        private void TryRaycastInteract()
        {
            if (Physics.Raycast(viewOrigin.position, viewOrigin.forward, out var hit, _playerParameters.interactionDistance, interactableMask))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    ClearInteractable();
                    return;
                }
                _currentInteractable = interactable;
                _currentInteractable.GameObject.layer = LayerMask.NameToLayer("Outline");
                return;
            }
            ClearInteractable();
        }

        private void ClearInteractable()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.GameObject.layer = LayerMask.NameToLayer("Default");
            }
            _currentInteractable = null;
        }

        private void InteractionStarted()
        {
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
            ClearInteractable();
        }

        private void InteractionCanceled()
        {
            if (_currentInteractable is IHoldInteractable holdInteractable)
            {
                holdInteractable.OnHoldCanceled(this);
            }
            ClearInteractable();
        }
    }
}