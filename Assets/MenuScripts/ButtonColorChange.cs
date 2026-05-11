using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TMPHoverEffect : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public TextMeshProUGUI text;

    private Material mat;

    public float normalGlow = 0.5f;
    public float hoverGlow = 1f;

    public float normalOutline = 0f;
    public float hoverOutline = 0.6f;

    void Start()
    {
        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();

        if (text == null)
        {
            Debug.LogError("TMPHoverEffect: TextMeshProUGUI not found!", gameObject);
            return;
        }

        mat = Instantiate(text.fontMaterial);
        text.fontMaterial = mat;

        ApplyNormal();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mat == null)
            return;

        mat.SetFloat(ShaderUtilities.ID_GlowPower, hoverGlow);
        mat.SetFloat(ShaderUtilities.ID_OutlineWidth, hoverOutline);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mat == null)
            return;

        ApplyNormal();
    }

    void ApplyNormal()
    {
        mat.SetFloat(ShaderUtilities.ID_GlowPower, normalGlow);
        mat.SetFloat(ShaderUtilities.ID_OutlineWidth, normalOutline);
    }
}