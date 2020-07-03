using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGUI : MonoBehaviour
{
    private ObjectManager om;
    [SerializeField] private Text killsText;

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
        //Implementare aggiornamento parti restanti dell'interfaccia

        //Aggiornamento barra della vita dei nemici
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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

        }

    }
}
