using UnityEngine;
using UnityEngine.UI;

public class CombatantStatusUI : MonoBehaviour
{
    private Combatant combatant;
    private Text nameText;
    private Text healthText;
    private Slider healthSlider;

    public void Initialize(Combatant target, string displayName)
    {
        combatant = target;

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
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        return text;
    }

    private Slider CreateSlider()
    {
        // This is complex to create from scratch, so we'll just make a simple bar
        GameObject sliderGO = new GameObject("HealthSlider");
        sliderGO.transform.SetParent(transform, false);
        Image bg = sliderGO.AddComponent<Image>();
        bg.color = Color.red;

        GameObject fillGO = new GameObject("Fill");
        fillGO.transform.SetParent(sliderGO.transform, false);
        Image fillImg = fillGO.AddComponent<Image>();
        fillImg.color = Color.green;

        Slider slider = sliderGO.AddComponent<Slider>();
        slider.fillRect = fillGO.GetComponent<RectTransform>();
        slider.targetGraphic = fillImg;

        RectTransform bgRect = sliderGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(200, 20);
        RectTransform fillRect = fillGO.GetComponent<RectTransform>();
        fillRect.sizeDelta = new Vector2(200, 20);

        return slider;
    }
}
