using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Combatant player;
    public Combatant enemy;

    [Header("UI Panels")]
    public Image playerPanel;
    public Image enemyPanel;
    public Image dialoguePanel;

    [Header("Player Stats")]
    public Slider playerStaminaSlider;
    public Text playerStaminaText;
    public Slider playerCredibilitySlider;
    public Text playerCredibilityText;
    public Slider playerPASlider;
    public Text playerPAText;

    [Header("Enemy Stats")]
    public Slider enemyStaminaSlider;
    public Text enemyStaminaText;
    public Slider enemyCredibilitySlider;
    public Text enemyCredibilityText;

    [Header("Asset Paths")]
    public string playerSpritePath = "Characters/PNG/Default/blue_body_square";
    public string enemySpritePath = "Characters/PNG/Default/red_body_square";
    public string buttonSpritePath = "UI/PNG/Blue/Default/button_rectangle_flat";
    public string panelSpritePath = "UI/PNG/Blue/Default/panel_background"; // Assuming a panel background image exists

    void Start()
    {
        LoadAssets();
        SetupUI();
    }

    void LoadAssets()
    {
        // Load player and enemy sprites
        Sprite playerSprite = Resources.Load<Sprite>(playerSpritePath);
        if (playerSprite != null && player.characterSprite != null)
        {
            player.SetSprite(playerSprite);
        }

        Sprite enemySprite = Resources.Load<Sprite>(enemySpritePath);
        if (enemySprite != null && enemy.characterSprite != null)
        {
            enemy.SetSprite(enemySprite);
        }

        // Load UI sprites
        Sprite buttonSprite = Resources.Load<Sprite>(buttonSpritePath);
        Sprite panelSprite = Resources.Load<Sprite>(panelSpritePath);

        // Apply sprites to UI elements (example)
        if (playerPanel != null && panelSprite != null)
        {
            playerPanel.sprite = panelSprite;
        }
        if (enemyPanel != null && panelSprite != null)
        {
            enemyPanel.sprite = panelSprite;
        }
    }

    void SetupUI()
    {
        // Initialize sliders
        if (playerStaminaSlider != null)
        {
            playerStaminaSlider.maxValue = player.maxEmotionalStamina;
        }
        if (playerCredibilitySlider != null)
        {
            playerCredibilitySlider.maxValue = player.maxCredibility;
        }
        if (playerPASlider != null)
        {
            playerPASlider.maxValue = 100; // Assuming max PA is 100
        }
        if (enemyStaminaSlider != null)
        {
            enemyStaminaSlider.maxValue = enemy.maxEmotionalStamina;
        }
        if (enemyCredibilitySlider != null)
        {
            enemyCredibilitySlider.maxValue = enemy.maxCredibility;
        }
    }

    void Update()
    {
        if (player != null)
        {
            UpdateSliderAndText(playerStaminaSlider, playerStaminaText, player.currentEmotionalStamina, player.maxEmotionalStamina, "ES");
            UpdateSliderAndText(playerCredibilitySlider, playerCredibilityText, player.currentCredibility, player.maxCredibility, "Cred");
            //UpdateSliderAndText(playerPASlider, playerPAText, player.passiveAggressiveMeter, 100, "PA");
        }
        if (enemy != null)
        {
            UpdateSliderAndText(enemyStaminaSlider, enemyStaminaText, enemy.currentEmotionalStamina, enemy.maxEmotionalStamina, "ES");
            UpdateSliderAndText(enemyCredibilitySlider, enemyCredibilityText, enemy.currentCredibility, enemy.maxCredibility, "Cred");
        }
    }

    void UpdateSliderAndText(Slider slider, Text text, int currentValue, int maxValue, string prefix)
    {
        if (slider != null)
        {
            slider.value = currentValue;
        }
        if (text != null)
        {
            text.text = $"{prefix}: {currentValue}/{maxValue}";
        }
    }
}
