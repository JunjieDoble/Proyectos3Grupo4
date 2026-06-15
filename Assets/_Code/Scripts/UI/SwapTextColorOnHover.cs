using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class SwapTextColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color hoverTextColor;
    private Color originalTextColor;

    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            originalTextColor = textMeshPro.color;
        }

        hoverTextColor = new Color(129f / 255f, 1f, 1f);
    }

    private void OnDisable()
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = originalTextColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshPro != null)
            textMeshPro.color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshPro != null)
            textMeshPro.color = originalTextColor;
    }
}
