using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _GameState { Play, Pause, Over, Won }
public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    private ObjectManager om;

    private bool gameStarted = false;
    private float playerLife;
    public _GameState gameState = _GameState.Play;
    private string nextScene = "SampleScene";
    private string mainMenu = "MainMenuScene";

    private int levelReached;

    public static int LevelReached()
    {
        instance.levelReached = PlayerPrefs.GetInt("levelReached", 1);
        return instance.levelReached;
    }
    public static bool IsGameStarted() { 
        return instance.gameStarted; 
    }
    public static bool IsGameOver() { 
        return instance.gameState == _GameState.Over ? true : false; 
    }
    public static bool IsGamePaused() {
        return instance.gameState == _GameState.Pause ? true : false; 
    }

    public static float GetPlayerLife() { return instance.playerLife; }

    public static _GameState GetGameState() { 
        return instance.gameState; 
    }
    public static void SetGameStateOver()
    {
        instance.gameState = _GameState.Over;
        Time.timeScale = 0;
    }
    public static bool HasPlayerWon() {
        return instance.gameState == _GameState.Won ? true : false;
    }
    public static void SetGameStateWon()
    {
        instance.gameState = _GameState.Won;
        Time.timeScale = 0;
    }

    public static void LoadNextScene() { 
        if (instance.nextScene != "") 
            Load(instance.nextScene); 
    }
    public static void LoadMainMenu() { 
        if (instance.mainMenu != "") 
            Load(instance.mainMenu); 
    }
    public static void Load(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
    void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //* implementare singleton

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        playerLife = om.FoodStamina;
    }


    public static void StartGame()
    {
        instance.gameStarted = true;
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
