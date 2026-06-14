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
        originalTextColor = textMeshPro.color;
        hoverTextColor = new Color(129f / 255f, 1f, 1f);
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
