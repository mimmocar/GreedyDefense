using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGUI : MonoBehaviour
{
    private ObjectManager om;
    
    [SerializeField] private Text killsText;
    [SerializeField] private Text currency;
    [SerializeField] private Image foodStamina;
    [SerializeField] private GameObject berserkText;
    private Text berserkTxt;
    private float startFoodStamina;
    private float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //implementare singleton
        berserkTxt = berserkText.GetComponent<Text>();
        startFoodStamina = om.StartFoodStamina;
        killsText.text = 0.ToString() + "/" + om.Berserk.ToString();
        foodStamina.fillAmount = om.FoodStamina / startFoodStamina;
        Messenger.AddListener(GameEvent.BERSERK_ON, OnBerserkOn);
        Messenger.AddListener(GameEvent.BERSERK_OFF, OnBerserkOff);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BERSERK_ON, OnBerserkOn);
        Messenger.RemoveListener(GameEvent.BERSERK_OFF, OnBerserkOff);
    }

    private void OnBerserkOn()
    {
        berserkText.SetActive(true);
        berserkTxt.text = "";
    }

    private void OnBerserkOff()
    {
        berserkText.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        //Aggiornamento Kills
        killsText.text = om.Kills.ToString() + "/" + om.Berserk.ToString();
        currency.text = om.Currency.ToString(); //implementare conversione a intero della currency
        //Implementare aggiornamento parti restanti dell'interfaccia

        //Aggiornamento FoodStamina
        float fS = om.FoodStamina;
        
        float startStam = foodStamina.fillAmount;
        float endStam = fS / startFoodStamina;

        foodStamina.fillAmount = Mathf.Lerp(startStam, endStam, speed * Time.deltaTime);

        if (berserkText.activeSelf)
        {
            berserkTxt.text = ""+om.CountDown.ToString();
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
