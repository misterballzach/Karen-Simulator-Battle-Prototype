using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Entity player;
    public Entity enemy;

    public Text playerHealthText;
    public Text playerManaText;
    public Text enemyHealthText;
    public Text enemyManaText;

    void Update()
    {
        if (player != null)
        {
            playerHealthText.text = $"Player HP: {player.currentHealth}/{player.maxHealth}";
            playerManaText.text = $"Player Mana: {player.currentMana}/{player.maxMana}";
        }
        if (enemy != null)
        {
            enemyHealthText.text = $"Enemy HP: {enemy.currentHealth}/{enemy.maxHealth}";
            enemyManaText.text = $"Enemy Mana: {enemy.currentMana}/{enemy.maxMana}";
        }
    }
}
