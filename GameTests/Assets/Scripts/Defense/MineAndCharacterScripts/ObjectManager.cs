using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    //Start is called before the first frame update

    private int startCurrency = 70; //implementare lettura da file
    private int currentCurrency;
    private int hit = 0, kills = 0, berserk = 100;
    [SerializeField] GameObject[] prefab;

    public int Kills
    {
        get
        {
            return kills;
        }
    }

    public int Berserk
    {
        get
        {
            return berserk;
        }
    }

    public float Currency
    {
        get
        {
            return currentCurrency;
        }
    }
    
    void Awake()
    {
        currentCurrency = startCurrency;
        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.AddListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
        Debug.Log("LETTURA FEATURES COST "+prefab[0].GetComponent<Features>().Cost);

    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.RemoveListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void OnSpawnObject(Vector3 position, int i)
    {
        float cost = prefab[i].GetComponent<Features>().Cost;
        if (currentCurrency >= cost)
        {
            position += new Vector3(0, prefab[i].transform.localScale.y / 2, 0);
            currentCurrency -= prefab[i].GetComponent<Features>().Cost;
            Instantiate(prefab[i], position, new Quaternion(0, 0, 0, 0));
        }
    }

    public int GetCost(int i)
    {
        return prefab[i].GetComponent<Features>().Cost;
    }

    void OnHandleDamage(GameObject target, _Damage damage)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        hit += 1;
        Debug.Log("Invocazione n " + hit);

        if (enemy != null)
        {
            float[] damagesMultipilers = enemy.DamagesMultipliers;
            float amount = damage.Amount;
            int index = (int)damage.Type;

            enemy.Health = enemy.Health - damagesMultipilers[index] * amount;
            
            Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<Enemy>().Health);
            if (enemy.Dead)
            {
                enemy.Die();
                currentCurrency += enemy.Worth; //implementare lettura valore nemico

                if (kills >= berserk)
                {
                    kills = 0;
                }
                kills++;
            }
        }

    }
}
