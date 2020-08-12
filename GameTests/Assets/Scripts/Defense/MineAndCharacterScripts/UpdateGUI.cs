using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGUI : MonoBehaviour
{
    private ObjectManager om;
    [SerializeField] private Text killsText;
    [SerializeField] private Text currency;

    // Start is called before the first frame update
    void Start()
    {
        om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //implementare singleton
        killsText.text = 0.ToString() + "/" + om.Berserk.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Aggiornamento Kills
        killsText.text = om.Kills.ToString() + "/" + om.Berserk.ToString();
        currency.text = om.Currency.ToString(); //implementare conversione a intero della currency
        //Implementare aggiornamento parti restanti dell'interfaccia

        //Aggiornamento barra della vita dei nemici
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
<<<<<<< HEAD
        foreach(GameObject en in enemies)
        {
            Enemy enemy = en.GetComponent<Enemy>();

            if(enemy != null)
            {
                float health = enemy.Health;
                Image healthBar = enemy.healthBar;
                float startH = enemy.startHealth;
                //healthBar.fillAmount = health / startH;
                
                float speed = 2f;
                float start = healthBar.fillAmount;
                float end = health / startH;

                healthBar.fillAmount = Mathf.Lerp(start, end, speed * Time.deltaTime);


            }

=======
        foreach(GameObject en in enemies)
        {
            Enemy enemy = en.GetComponent<Enemy>();

            if(enemy != null)
            {
                float health = enemy.Health;
                Image healthBar = enemy.healthBar;
                float startH = enemy.startHealth;
                healthBar.fillAmount = health / startH;
            }

>>>>>>> ae71a95e0d1272633363b42e3feaef3c68a0965b
        }

    }
}
