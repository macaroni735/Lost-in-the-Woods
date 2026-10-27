using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text buttonText;
    private Text legacyButtonText;
    private Color originalColor;
    public Color hoverColor = Color.red;

    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>(true);
        if (buttonText != null)
            originalColor = buttonText.color;
        else
        {
            legacyButtonText = GetComponentInChildren<Text>(true);
            if (legacyButtonText != null)
                originalColor = legacyButtonText.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
        else if (legacyButtonText != null)
            legacyButtonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = originalColor;
        else if (legacyButtonText != null)
            legacyButtonText.color = originalColor;
    }
}
