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
            if (context.started) TryRaycastInteract();
            if (context.performed) ObjectInteract();
            if (context.canceled) CancelInteraction();
        }

        private bool TryRaycastInteract()
        {
            //Debug.DrawRay(viewOrigin.position, viewOrigin.forward * maxDistance, Color.red, 1f);

            if (Physics.Raycast(viewOrigin.position, viewOrigin.forward, out var hit, maxDistance, interactableMask))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable == null) return false;
                if (!interactable.CanInteract()) return false;
                interactable.Interact();
                _currentInteractable = interactable;
                return true;
            }

            return false;
        }

        private void ObjectInteract()
        {
            Debug.Log("Hold Finished");
            _currentInteractable = null;
        }

        private void CancelInteraction()
        {
            _currentInteractable?.CancelInteract();
            _currentInteractable = null;
        }
    }
}