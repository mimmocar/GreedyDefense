using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Runtime.CompilerServices;

public enum _GameState { Play, Pause, Over, Won, Restart }
public enum OperationRequested { Home, Continue, SelectLevel, Weapon, Retry, Null }
public class GameControl : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    private static GameControl instance;
    private ObjectManager om;
    private WaveSpawner spawner;
    private bool gameStarted = false;
    private float playerLife;
    public _GameState gameState = _GameState.Play;

    public int currentLevel; //Da testare: provare a leggere il numero del livello dal nome della scena o dall'indice
    private float firstTH, secondTH;
    private int levelReached;
    private int score = 0;

    private OperationRequested op = OperationRequested.Null;
    public float FirstTh
    {
        get
        {
            return firstTH;
        }
    }

    public float SecondTH
    {
        get
        {
            return secondTH;
        }
    }
    public int Score
    {
        get
        {
            return score;
        }
    }
    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
    }

    public static GameControl Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<GameControl>();
        return instance;
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
    public bool IsGameOver()
    {
        return gameState == _GameState.Over ? true : false;
    }
    public bool IsGamePaused()
    {
        return gameState == _GameState.Pause ? true : false;
    }

    public float GetPlayerLife() { return instance.playerLife; }

    public _GameState GetGameState()
    {
        return gameState;
    }
    public void SetGameStateOver()
    {
        gameState = _GameState.Over;
        Time.timeScale = 0;
    }
    public bool HasPlayerWon()
    {
        return gameState == _GameState.Won ? true : false;
    }
    public void SetGameStateWon()
    {
        gameState = _GameState.Won;
        Time.timeScale = 0;
    }


    public OperationRequested OpRequested
    {

        set
        {
            op = value;
        }


    }

    public static void LoadMainMenu()
    {
        Time.timeScale = 1;
        Load("MainMenuScene");
    }
    public static void Load(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    void Awake()
    {
        //instance = this;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        om = ObjectManager.Instance(); //* implementare singleton
        spawner = GetComponent<WaveSpawner>();
        levelReached = PlayerPrefs.GetInt("levelReached", 1);

        string filePath = "File/Level" + currentLevel + "Features";

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
        if (gameState == _GameState.Restart)
            Load(SceneManager.GetActiveScene().name);


        playerLife = om.FoodStamina;

        if (playerLife <= 0)
            EndGame();
        else if (spawner.HasPlayerWon)
            WonGame();

        switch (op)
        {
            case OperationRequested.Home: LoadMainMenu(); break;
            case OperationRequested.Continue:
                StoreSkulls();
                //ResumeGame();
                Load("Level" + (currentLevel + 1)); break;
            case OperationRequested.SelectLevel:
                if (gameState == _GameState.Won)
                    StoreSkulls();
                //ResumeGame();
                Load("LevelSelector"); break;
            case OperationRequested.Retry:
                //ResumeGame();
                Load(SceneManager.GetActiveScene().name);
                break;
            case OperationRequested.Weapon: LoadWeapon(); op = OperationRequested.Null; break;
            default: break;
        }

    }


    public void StartGame()
    {
        gameStarted = true;
    }

    public void PauseGame()
    {
        gameState = _GameState.Pause;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        gameState = _GameState.Play;
        Time.timeScale = 1;
        Debug.Log("GAME RESUMED");
    }


    void EndGame()
    {

        SetGameStateOver();
        Debug.Log("Game Over");

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


    }





    private void StoreSkulls()
    {
        PlayerPrefs.SetInt("skullsCurrency", om.Skulls);
    }
    public void SelectLevel()
    {
        //if(gameState == _GameState.Won)
        //    StoreSkulls();
        //ResumeGame();
        //Load("LevelSelector");
        op = OperationRequested.SelectLevel;
    }
    public void Retry()
    {
        //ResumeGame();
        //Load(SceneManager.GetActiveScene().name);
        op = OperationRequested.Retry;
    }

    public void Continue()
    {
        //StoreSkulls();
        //ResumeGame();
        //Load("Level" + (currentLevel + 1));
        op = OperationRequested.Continue;
    }

    public void SelectWeapon()
    {
        //ResumeGame();
        //LoadWeapon();
        op = OperationRequested.Weapon;

    }

    public static void LoadWeapon()
    {
        SceneManager.LoadScene("WeaponSelection", LoadSceneMode.Additive);
    }

    public static void UnloadWeapon()
    {
        SceneManager.UnloadScene("WeaponSelection");
    }

    public static void Quit()
    {
        Application.Quit();
    }



}
