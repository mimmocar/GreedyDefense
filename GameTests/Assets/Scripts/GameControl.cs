using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _GameState { Play, Pause, Over }
public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    private bool gameStarted = false;
    public static bool IsGameStarted() { return instance.gameStarted; }
    public static bool IsGameOver() { return instance.gameState == _GameState.Over ? true : false; }
    public static bool IsGamePaused() { return instance.gameState == _GameState.Pause ? true : false; }
    public _GameState gameState = _GameState.Play;
    public static _GameState GetGameState() { return instance.gameState; }

    private bool playerWon = false;
    public static bool HasPlayerWon() { return instance.playerWon; }

    private string nextScene = "SampleScene";
    private string mainMenu = "MainMenuScene";
    public static void LoadNextScene() { if (instance.nextScene != "") Load(instance.nextScene); }
    public static void LoadMainMenu() { if (instance.mainMenu != "") Load(instance.mainMenu); }
    public static void Load(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }
    public static void StartGame()
    {
        instance.gameStarted = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PauseGame()
    {
        instance.gameState = _GameState.Pause;
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        instance.gameState = _GameState.Play;
        Time.timeScale = 1;
    }
}
