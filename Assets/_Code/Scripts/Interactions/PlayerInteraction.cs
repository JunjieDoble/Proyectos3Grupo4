using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactions
{
    [RequireComponent(typeof(PlayerInput))]
    [DisallowMultipleComponent]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Raycast")]
        [SerializeField] private Transform viewOrigin;
        [SerializeField] private float maxDistance = 3f;
        [SerializeField] private LayerMask interactableMask;

        private IInteractable _currentInteractable;

        private void Awake()
        {
            if (viewOrigin == null)
            {
                Camera cam = Camera.main;
                viewOrigin = cam != null ? cam.transform : transform;
            }
            if (interactableMask.value == 0) Debug.LogWarning("Unassigned interactableMask in PlayerInteraction");
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started && TryRaycastInteract()) InteractionStarted();
            if (context.performed && _currentInteractable != null) InteractionPerformed();
            if (context.canceled && _currentInteractable != null) InteractionCanceled();
        }

        private bool TryRaycastInteract()
        {
            //Debug.DrawRay(viewOrigin.position, viewOrigin.forward * maxDistance, Color.red, 1f);

            if (Physics.Raycast(viewOrigin.position, viewOrigin.forward, out var hit, maxDistance, interactableMask))
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
            _currentInteractable?.Interact();
        }

        private void InteractionPerformed()
        {
            Debug.Log("Hold Finished");
            _currentInteractable = null;
        }

        private void InteractionCanceled()
        {
            if (_currentInteractable is IHoldInteractable holdInteractable)
            {
                holdInteractable?.OnHoldCanceled();
            }
            _currentInteractable = null;
        }
    }
}