using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGUI : MonoBehaviour
{
    private ObjectManager om;
    private GameControl gameControl;
    private PauseMenu pauseMenu;
    public Canvas canvas;
    [SerializeField] private Text killsText;
    [SerializeField] private Text currency;
    [SerializeField] private Text skulls;
    [SerializeField] private Image foodStamina;
    [SerializeField] private Image foodStaminaScore;
    [SerializeField] private GameObject berserkText;
    [SerializeField] private GameObject berserkSignal;
    [SerializeField] private Text waveCountdown; 
    [SerializeField] private Text waveCounter;
    [SerializeField] private Image waveSignal;
    [SerializeField] GameObject joystickGO;
    [SerializeField] GameObject buttonGO;
    [SerializeField] VariableJoystick joystick;
    [SerializeField] FloatingButton shootingButton;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject levelWonUI;
    private JoystickCharacterState playerStatus;
    private Text berserkTxt;
    private float startFoodStamina;
    private float speed = 5f;
    private float scoreSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        //om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //implementare singleton
        om = ObjectManager.Instance();
        gameControl = GameControl.Instance();
        pauseMenu = PauseMenu.Instance();
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<JoystickCharacterState>();
        berserkTxt = berserkText.GetComponent<Text>();
        startFoodStamina = om.StartFoodStamina;
        killsText.text = 0.ToString() + "/" + om.Berserk.ToString();
        foodStamina.fillAmount = om.FoodStamina / startFoodStamina;
        skulls.text = om.Skulls.ToString(); 
        waveCounter.text = om.CurrentWave.ToString() + "/" + om.WavesNum.ToString();

        string filePath = "File/updateGuiFeatures";
        

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "speed":
                    speed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "scoreSpeed":
                    scoreSpeed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }
        }

    }
    
    
    // Update is called once per frame
    void Update()
    {
        _GameState gameState = gameControl.GetGameState();
        float fS = om.FoodStamina;

        float startStam = foodStamina.fillAmount;
        float endStam = fS / startFoodStamina;

        foodStamina.fillAmount = Mathf.Lerp(startStam, endStam, speed * Time.fixedDeltaTime);

        if (gameState == _GameState.Play){
            if (pauseMenu.IsOn)
            {
                pauseMenu.Hide();
                //    joystickGO.SetActive(true);
                //if(!playerStatus.IsBerserkOn)
                //    buttonGO.SetActive(true);

            }

            if (playerStatus.ReadScreeInput)
            {
                joystickGO.SetActive(true);
                if (!playerStatus.IsBerserkOn)
                    buttonGO.SetActive(true);
            }
            else
            {
                joystickGO.SetActive(false);
                buttonGO.SetActive(false);
            }


            //Aggiornamento Kills
            killsText.text = om.Kills.ToString() + "/" + om.Berserk.ToString();
            currency.text = om.Currency.ToString(); //implementare conversione a intero della currency
                                                    //Implementare aggiornamento parti restanti dell'interfaccia
            waveCounter.text = om.CurrentWave.ToString() + "/" + om.WavesNum.ToString();
            skulls.text = om.Skulls.ToString();
            if (om.WaveCountdown < 1)
            {
                waveCountdown.gameObject.SetActive(false);
                waveSignal.gameObject.SetActive(false);
                
            }
            else
            {
                waveCountdown.gameObject.SetActive(true);
                waveSignal.gameObject.SetActive(true);
            }
            waveCountdown.text = "Next wave in " + om.WaveCountdown.ToString();

            

            if (playerStatus.IsBerserkOn)

            {
                berserkSignal.SetActive(true);
                berserkText.SetActive(true);
                berserkTxt.text = "" + om.CountDown.ToString();
            }
            else
            {
                berserkSignal.SetActive(false);
                berserkText.SetActive(false);
            }
            //Aggiornamento barra della vita dei nemici
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject en in enemies)
            {
                Enemy enemy = en.GetComponent<Enemy>();

                if (enemy != null)
                {
                    float health = enemy.Health;
                    Image healthBar = enemy.healthBar;
                    float startH = enemy.startHealth;
                    //healthBar.fillAmount = health / startH;


                    float start = healthBar.fillAmount;
                    float end = health / startH;

                    healthBar.fillAmount = Mathf.Lerp(start, end, speed * Time.deltaTime);


                }


            }
        }
        else if (gameState == _GameState.Pause)
        {
            if (!pauseMenu.IsOn)
            {
                pauseMenu.Show();
                joystickGO.SetActive(false);
                buttonGO.SetActive(false);
            }

                
        }
        else if(gameState == _GameState.Over)
        {
            gameOverUI.SetActive(true);
            joystickGO.SetActive(false);
            buttonGO.SetActive(false);
            berserkText.SetActive(false);
            berserkSignal.SetActive(false);
        }
        else if(gameState == _GameState.Won){
            StartCoroutine(LevelWon());
            //testing
            float finalFoodStamina = om.FoodStamina;
            Debug.Log("FOOD STAMINA: " + finalFoodStamina);
            Debug.Log("FOOD SCORE: " + gameControl.Score);
        }

        
    }

    private IEnumerator LevelWon()
    {
        levelWonUI.SetActive(true);
        joystickGO.SetActive(false);
        buttonGO.SetActive(false);
        berserkText.SetActive(false);
        berserkSignal.SetActive(false);

        yield return new WaitForSecondsRealtime(2f);

        float firstTH = gameControl.FirstTh;
        float secondTH = gameControl.SecondTH;
        GameObject[] starBorders = GameObject.FindGameObjectsWithTag("score");

        float finalFoodStamina = om.FoodStamina;
        //testing
        //float finalFoodStamina = 4.99f;
        float startStam = foodStaminaScore.fillAmount;
        float endStam = finalFoodStamina / startFoodStamina;

        foodStaminaScore.fillAmount = Mathf.Lerp(startStam, endStam, scoreSpeed * Time.fixedDeltaTime);
        //foodStaminaScore.fillAmount = Mathf.Lerp(startStam, endStam, Mathf.SmoothStep(0.0f,1.0f,0.05f));
        while (foodStaminaScore.fillAmount >= endStam)
        {
            if(foodStaminaScore.fillAmount < secondTH)
            {
                starBorders[2].transform.GetChild(0).gameObject.SetActive(false);
            }
            if (foodStaminaScore.fillAmount < firstTH)
            {
                starBorders[1].transform.GetChild(0).gameObject.SetActive(false);
            }

            startStam = foodStaminaScore.fillAmount;
            //endStam = finalFoodStamina / startFoodStamina;

            foodStaminaScore.fillAmount = Mathf.Lerp(startStam, endStam, scoreSpeed * Time.fixedDeltaTime);
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        Debug.Log("FOOD STAMINA: " + finalFoodStamina);
        Debug.Log("FOOD STAMINA SCORE: " + foodStaminaScore);
        StopCoroutine(LevelWon());
        
    }
}
