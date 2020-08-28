using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Runtime.CompilerServices;

public enum _GameState { Play, Pause, Over, Won }
public class GameControl : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    public static GameControl instance;
    private ObjectManager om;
    private WaveSpawner spawner;
    private bool gameStarted = false;
    private float playerLife;
    public _GameState gameState = _GameState.Play;
    //private string nextScene = "Level1";
    //private string mainMenu = "MainMenuScene";
    public int currentLevel = 1; //test
    private float firstTH, secondTH;
    private int levelReached;
    private int score = 0;

    
    public static float FirstTh
    {
        get
        {
            return instance.firstTH;
        }
    }

    public static float SecondTH
    {
        get
        {
            return instance.secondTH;
        }
    }
    public static int Score
    {
        get
        {
            return instance.score;
        }
    }
    public static int CurrentLevel
    {
        get
        {
            return instance.currentLevel;
        }
    }

    public static bool IsGameStarted()
    {
        return instance.gameStarted;
    }
    public static bool IsGameOver()
    {
        return instance.gameState == _GameState.Over ? true : false;
    }
    public static bool IsGamePaused()
    {
        return instance.gameState == _GameState.Pause ? true : false;
    }

    public static float GetPlayerLife() { return instance.playerLife; }

    public static _GameState GetGameState()
    {
        return instance.gameState;
    }
    public static void SetGameStateOver()
    {
        instance.gameState = _GameState.Over;
        Time.timeScale = 0;
    }
    public static bool HasPlayerWon()
    {
        return instance.gameState == _GameState.Won ? true : false;
    }
    public static void SetGameStateWon()
    {
        instance.gameState = _GameState.Won;
        Time.timeScale = 0;
    }


    public static void LoadMainMenu()
    {
        Load("MainMenuScene");
    }
    public static void Load(string levelName)
    {
        SceneManager.LoadScene(levelName);
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
        spawner = GetComponent<WaveSpawner>();
        levelReached = PlayerPrefs.GetInt("levelReached", 1);

        string filePath = "File/Level" + currentLevel + "Thresholds";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "firstTH":
                    firstTH = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "secondTH":
                    secondTH = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }

        }

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        playerLife = om.FoodStamina;

        if (playerLife <= 0)
            EndGame();
        else if (spawner.HasPlayerWon)
            WonGame();

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


    void EndGame()
    {

        SetGameStateOver();
        Debug.Log("Game Over");
        //Messenger.Broadcast(GameEvent.GAME_OVER);
    }

    void WonGame()
    {
        SetGameStateWon();
        //int score = 0;
        float finalStamina = om.FoodStamina;
        float startStamina = om.StartFoodStamina;
        float finalStaminaPercentage = finalStamina / startStamina;
        if (finalStaminaPercentage < firstTH)
        {
            score = 1;
        }
        else if (finalStaminaPercentage >= firstTH && finalStaminaPercentage < secondTH)
        {
            score = 2;
        }
        else if (finalStaminaPercentage >= secondTH)
        {
            score = 3;
        }


        int bestScore = PlayerPrefs.GetInt("starForLevel" + currentLevel, 0);

        if (score > bestScore)
            PlayerPrefs.SetInt("starForLevel" + currentLevel, score);


        if (currentLevel == levelReached)
        { //aggiorno il livello raggiunto solo se quello che è terminato è proprio l'ultimo livello raggiunto. I livelli si possono rigiocare
            levelReached += 1;
            PlayerPrefs.SetInt("levelReached", levelReached);
        }

        //PlayerPrefs.SetInt("skullsCurrency", om.Skulls);

        //Messenger<int>.Broadcast(GameEvent.LEVEL_WON, score);
    }


    private void StoreSkulls()
    {
        PlayerPrefs.SetInt("skullsCurrency", om.Skulls);
    }
    public void SelectLevel()
    {
        StoreSkulls();
        ResumeGame();
        Load("LevelSelector");
    }
    public void Retry()
    {
        ResumeGame();
        Load(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        StoreSkulls();
        ResumeGame();
        Load("Level" + (currentLevel + 1));
    }

    public void SelectWeapon()
    {
        //ResumeGame();
        LoadWeapon();

    }

    public static void LoadWeapon()
    {
        SceneManager.LoadScene("WeaponSelection", LoadSceneMode.Additive);
    }

    public static void UnloadWeapon()
    {
        SceneManager.UnloadScene("WeaponSelection");
    }



}
