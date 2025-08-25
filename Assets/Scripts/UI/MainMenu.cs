using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
        else
        {
            Debug.LogError("Start Button not assigned in MainMenu script.");
        }
    }

    void OnStartButtonClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartBattle();
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClick);
        }
    }
}
