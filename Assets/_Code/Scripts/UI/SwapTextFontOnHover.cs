using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapTextFontOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_FontAsset hoverFont;

    private TMP_FontAsset originalFont;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            originalFont = textMeshPro.font;
        }
    }

    private void OnDisable()
    {
        if (textMeshPro != null && originalFont != null)
        {
            textMeshPro.font = originalFont;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshPro != null && hoverFont != null)
        {
            textMeshPro.font = hoverFont;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshPro != null && originalFont != null)
        {
            textMeshPro.font = originalFont;
        }
    }
}
