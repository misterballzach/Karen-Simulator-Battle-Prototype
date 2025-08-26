using UnityEngine;
using UnityEngine.UI;

public class CombatantStatusUI : MonoBehaviour
{
    private Combatant combatant;
    private Text nameText;
    private Text healthText;
    private Slider healthSlider;
    private Font uiFont;

    public void Initialize(Combatant target, string displayName, Font font)
    {
        combatant = target;
        uiFont = font;

        gameObject.AddComponent<VerticalLayoutGroup>();
        nameText = CreateText(displayName, 24);
        healthSlider = CreateSlider();
        healthText = CreateText("100/100", 18);
    }

    void Update()
    {
        if (combatant != null)
        {
            float healthPercent = (float)combatant.currentEmotionalStamina / combatant.maxEmotionalStamina;
            healthSlider.value = healthPercent;
            healthText.text = $"{combatant.currentEmotionalStamina} / {combatant.maxEmotionalStamina}";
        }
    }

    private Text CreateText(string content, int fontSize)
    {
        GameObject textGO = new GameObject("StatusText");
        textGO.transform.SetParent(transform, false);
        Text text = textGO.AddComponent<Text>();
        text.text = content;
        text.font = uiFont != null ? uiFont : Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = Color.black;
        return text;
    }

    private Slider CreateSlider()
    {
        // This is a simplified way to create a slider from code.
        // A real implementation would likely use a prefab for more complex visuals.
        GameObject sliderGO = new GameObject("HealthSlider");
        sliderGO.transform.SetParent(transform, false);
        Image bg = sliderGO.AddComponent<Image>();
        bg.color = new Color(0.5f, 0, 0, 0.5f); // Dark red background

        GameObject fillArea = new GameObject("Fill Area");
        fillRectArea.transform.SetParent(sliderGO.transform, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.sizeDelta = Vector2.zero;

        GameObject fillGO = new GameObject("Fill");
        fillGO.transform.SetParent(fillArea.transform, false);
        Image fillImg = fillGO.AddComponent<Image>();
        fillImg.color = Color.green;

        Slider slider = sliderGO.AddComponent<Slider>();
        slider.fillRect = fillGO.GetComponent<RectTransform>();
        slider.targetGraphic = fillImg;
        slider.transition = Selectable.Transition.None;

        RectTransform bgRect = sliderGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(200, 20);

        return slider;
    }
}
