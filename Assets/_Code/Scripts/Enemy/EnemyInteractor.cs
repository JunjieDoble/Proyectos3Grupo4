using _Code.Scripts.Character;
using _Code.Scripts.Interactions;
using _Code.Scripts.Pickupables;
using Interactions;
using UnityEngine;

public class EnemyInteractor : MonoBehaviour, IInteractor
{
    [SerializeField] private LayerMask interactableMask;

    private IInteractable _currentInteractable;

    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    public void AssignInteractable(IInteractable interactable)
    {
        _currentInteractable = interactable;
    }

    public void OnInteract()
    {
        if (_currentInteractable == null) return;
        Debug.Log(gameObject.name + ": OnInteract() was called by " + _currentInteractable);
        _currentInteractable?.Interact(this);
    }
}
