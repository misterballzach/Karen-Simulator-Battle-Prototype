using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Combatant player;
    public Combatant enemy;

    [Header("Player Stats")]
    public Text playerStaminaText;
    public Text playerCredibilityText;
    public Text playerPAText; // Passive-Aggressive

    [Header("Enemy Stats")]
    public Text enemyStaminaText;
    public Text enemyCredibilityText;

    void Update()
    {
        if (player != null)
        {
            playerStaminaText.text = $"ES: {player.currentEmotionalStamina}/{player.maxEmotionalStamina}";
            playerCredibilityText.text = $"Cred: {player.currentCredibility}/{player.maxCredibility}";
            playerPAText.text = $"PA: {player.passiveAggressiveMeter}/{player.maxPassiveAggressive}";
        }
        if (enemy != null)
        {
            enemyStaminaText.text = $"ES: {enemy.currentEmotionalStamina}/{enemy.maxEmotionalStamina}";
            enemyCredibilityText.text = $"Cred: {enemy.currentCredibility}/{enemy.maxCredibility}";
        }
    }
}
