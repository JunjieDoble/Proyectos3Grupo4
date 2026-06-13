using TMPro;
using UnityEngine;

public class InteractionPromptTrigger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI interactionText;
    
    [Header("Text")]
    [SerializeField] private string message = "Press E to interact";

    private bool playerInside;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = true;
            if (interactionText != null)
            {
                interactionText.text = message;
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            
            if(interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
    
}
