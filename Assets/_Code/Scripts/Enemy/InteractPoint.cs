using Interactions;
using UnityEngine;

public class InteractPoint : MonoBehaviour
{
    [SerializeReference] private GameObject interactable;

    public GameObject GetInteractable()
    {
        return interactable;
    }
}
