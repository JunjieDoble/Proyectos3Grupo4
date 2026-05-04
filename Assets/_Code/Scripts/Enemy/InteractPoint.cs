using Interactions;
using UnityEngine;

public class InteractPoint : MonoBehaviour
{
    [SerializeField] private GameObject interactableObj;

    public IInteractable GetInteractable()
    {
        if (interactableObj == null) return null;

        IInteractable interactable = interactableObj.GetComponent<IInteractable>();
        return interactable;
    }
}
