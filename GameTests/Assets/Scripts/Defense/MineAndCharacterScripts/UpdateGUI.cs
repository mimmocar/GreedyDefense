using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGUI : MonoBehaviour
{
    private ObjectManager om;
    public Canvas canvas;
    [SerializeField] private Text killsText;
    [SerializeField] private Text currency;
    [SerializeField] private Text skulls;
    [SerializeField] private Image foodStamina;
    [SerializeField] private GameObject berserkText;
    [SerializeField] private Text waveCountdown; 
    [SerializeField] private Text waveCounter;
    private JoystickCharacterState playerStatus;
    private Text berserkTxt;
    private float startFoodStamina;
    private float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //implementare singleton
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<JoystickCharacterState>();
        berserkTxt = berserkText.GetComponent<Text>();
        startFoodStamina = om.StartFoodStamina;
        killsText.text = 0.ToString() + "/" + om.Berserk.ToString();
        foodStamina.fillAmount = om.FoodStamina / startFoodStamina;
        skulls.text = om.Skulls.ToString(); 
        waveCounter.text = om.CurrentWave.ToString() + "/" + om.WavesNum.ToString();
        
        Messenger.AddListener(GameEvent.LOAD_WEAPON_SELECTOR, OnLoadWeapon);
        Messenger.AddListener(GameEvent.UNLOAD_WEAPON_SELECTOR, OnUnLoadWeapon);
    }
    void OnDestroy()
    {
        
        Messenger.RemoveListener(GameEvent.LOAD_WEAPON_SELECTOR, OnLoadWeapon);
        Messenger.RemoveListener(GameEvent.UNLOAD_WEAPON_SELECTOR, OnUnLoadWeapon);
    }

    public void OnLoadWeapon()
    {

        canvas.enabled = false;

    }

    public void OnUnLoadWeapon()
    {
        canvas.enabled = true;
    }

    
    // Update is called once per frame
    void Update()
    {
        //Aggiornamento Kills
        killsText.text = om.Kills.ToString() + "/" + om.Berserk.ToString();
        currency.text = om.Currency.ToString(); //implementare conversione a intero della currency
        //Implementare aggiornamento parti restanti dell'interfaccia
        waveCounter.text = om.CurrentWave.ToString() + "/" + om.WavesNum.ToString();
        skulls.text = om.Skulls.ToString();
        if(om.WaveCountdown < 1)
        {
            waveCountdown.gameObject.SetActive(false);
        }
        else
        {
            waveCountdown.gameObject.SetActive(true);
        }
        waveCountdown.text = "Next wave in " + om.WaveCountdown.ToString();

        //Aggiornamento FoodStamina
        float fS = om.FoodStamina;
        
        float startStam = foodStamina.fillAmount;
        float endStam = fS / startFoodStamina;

        foodStamina.fillAmount = Mathf.Lerp(startStam, endStam, speed * Time.deltaTime);

        if (playerStatus.IsBerserkOn)

        {
            berserkText.SetActive(true);
            berserkTxt.text = ""+om.CountDown.ToString();
        }
        else
        {
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
}
