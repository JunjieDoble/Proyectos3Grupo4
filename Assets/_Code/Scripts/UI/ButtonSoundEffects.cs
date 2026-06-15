using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffects : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private FMODUnity.EventReference hoverSoundEvent;
    [SerializeField] private FMODUnity.EventReference clickSoundEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverSoundEvent.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(hoverSoundEvent);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clickSoundEvent.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(clickSoundEvent);
        }
    }
}
