using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    InBattle,
    BattleWon,
    BattleLost
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Starting state
        UpdateGameState(GameState.MainMenu);
    }

    public void UpdateGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                // Handle main menu logic, e.g., load main menu scene
                // Since we can't load scenes by name easily without the build settings,
                // we'll just log the state change for now.
                Debug.Log("Game State: Main Menu");
                break;
            case GameState.InBattle:
                Debug.Log("Game State: In Battle");
                break;
            case GameState.BattleWon:
                Debug.Log("Game State: Battle Won");
                break;
            case GameState.BattleLost:
                Debug.Log("Game State: Battle Lost");
                break;
        }
    }

    // Example function to start a battle
    public void StartBattle()
    {
        UpdateGameState(GameState.InBattle);
        // In a real game, you would load the battle scene here.
        // SceneManager.LoadScene("BattleScene");
    }

    // Example function to end a battle
    public void EndBattle(bool playerWon)
    {
        if (playerWon)
        {
            UpdateGameState(GameState.BattleWon);
        }
        else
        {
            UpdateGameState(GameState.BattleLost);
        }
        // In a real game, you might show a win/loss screen
        // and then return to the main menu.
        // SceneManager.LoadScene("MainMenuScene");
    }
}
