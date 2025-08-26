using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Text resultText;
    public Button restartButton;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }
        else
        {
            Debug.LogError("Restart Button not assigned in EndScreen script.");
        }

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.CurrentState == GameState.BattleWon)
            {
                resultText.text = "You Won!";
            }
            else if (GameManager.Instance.CurrentState == GameState.BattleLost)
            {
                resultText.text = "You Lost!";
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    void OnRestartButtonClick()
    {
        if (GameManager.Instance != null)
        {
            // This would typically load the main menu scene.
            // For now, we'll just set the state.
            GameManager.Instance.UpdateGameState(GameState.MainMenu);
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    private void OnDestroy()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }
    }
}
