using _Code.Scripts.Character;
using _Code.Scripts.Pickupables;
using Interactions;
using UnityEngine;

public class EnemyInteractor : MonoBehaviour, IInteractor, IController
{
    [SerializeField] private LayerMask interactableMask;

    private IInteractable _currentInteractable;
    private HoldablePickup _currentPickupable;

    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    public bool IsEnabled { get; set; }

    private void Awake()
    {
        if (interactableMask.value == 0) Debug.LogWarning("Unassigned interactableMask in EnemyInteractor");
    }

    public void AssignInteractable(IInteractable interactable)
    {
        _currentInteractable = interactable;
    }

    public void OnInteract()
    {
        if (_currentInteractable is ILockable lockable && lockable.IsLocked()) return;
        _currentInteractable?.Interact(this);
    }
}
