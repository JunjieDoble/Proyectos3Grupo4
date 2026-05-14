using System;
using Interactions;
using Unity.VisualScripting;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
