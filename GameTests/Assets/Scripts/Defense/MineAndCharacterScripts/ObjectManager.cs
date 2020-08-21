using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    //Start is called before the first frame update

    private int startCurrency = 70; //implementare lettura da file/PlayerPrefs
    private int currentCurrency;
    private int startSkulls = 0; //implementare lettura da file/PlayerPrefs
    private int currentSkulls;
    private int hit = 0, kills = 0, berserk = 100;  //inizializzazione all'inizio del livello
    [SerializeField] GameObject[] prefab;

    private float startFoodStamina = 100; //valore di inizializzazione costante
    private float foodStamina;
    private int countDown;

    private float waveCountdown; //*
    private int waves;
    private int currentWave;


    private int currentLevel;

    private bool gameEnded;

    public int WavesNum
    {
        get
        {
            return waves;
        }
        set
        {
            waves = value;
        }
    }

    public int CurrentWave
    {
        get
        {
            return currentWave;
        }
        set
        {
            currentWave = value;
        }
    }

    public float WaveCountdown
    {
        get
        {
            return waveCountdown;
        }
        set
        {
            waveCountdown = value;
        }
    }
    public bool GameIsOver
    {
        get
        {
            return gameEnded;
        }

        set
        {
            gameEnded = value;
        }
    }

    public int CountDown
    {
        get
        {
            return countDown;
        }
    }
   
    
    public int Kills
    {
        get
        {
            return kills;
        }
    }

    public float FoodStamina
    {
        get
        {
            return foodStamina;
        }
    }

    public float StartFoodStamina
    {
        get
        {
            return startFoodStamina;
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
        foodStamina = startFoodStamina;
        currentSkulls = startSkulls;

        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.AddListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
        Messenger.AddListener(GameEvent.HANDLE_FOOD_ATTACK, OnHandleFoodAttack);
        Debug.Log("LETTURA FEATURES COST "+prefab[0].GetComponent<Features>().Cost);


        //Aggiunto per motivi di testin
        kills = 97;
    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.RemoveListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
    }

    private void Start()
    {
        GameIsOver = false;
        currentLevel = GameControl.LevelReached();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver)
            return;
        if (this.FoodStamina <= 0)
            EndGame();

        if (GameControl.HasPlayerWon())
            WonGame();
    }

    void EndGame()
    {
        GameIsOver = true;
        GameControl.SetGameStateOver();
        Debug.Log("Game Over");
        Messenger.Broadcast(GameEvent.GAME_OVER);
    }
    
    void WonGame()
    {
        int score = 0;
        float finalStamina = this.FoodStamina / this.StartFoodStamina;
        if (finalStamina < 0.5)
        {
            score = 1;
            PlayerPrefs.SetInt("starForLevel" + currentLevel, score);
        }
        else if (finalStamina >= 0.5 && finalStamina < 0.75)
        {
            score = 2;
            PlayerPrefs.SetInt("starForLevel" + currentLevel, score);
        }
        else if (finalStamina >= 0.75)
        {
            score = 3;
            PlayerPrefs.SetInt("starForLevel" + currentLevel, score);
        }
        else
            PlayerPrefs.SetInt("starForLevel" + currentLevel, score);

        PlayerPrefs.SetInt("levelReached", currentLevel++);

        Messenger<int>.Broadcast(GameEvent.LEVEL_WON, score);
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
            if(damage.Type == DamageType.Berserk) //evito controllo perchè il nemici hanno il rigidbody
            {

                target.GetComponent<Rigidbody>().AddExplosionForce(1000.0f, target.transform.position, 10.0f, 0.0f, ForceMode.Force); //implementare lettura da file
                enemy.Health = 0;
                
                
            }
            else
            {
                //float[] damagesMultipilers = enemy.DamagesMultipliers;
                Dictionary<string, float> multipliers = enemy.DamagesMultiplierDic;
                float amount = damage.Amount;
                //int index = (int)damage.Type;
                string key = damage.Type.ToString() + "Multiplier";

                //enemy.Health = enemy.Health - damagesMultipilers[index] * amount;
                enemy.Health = enemy.Health - multipliers[key] * amount;
            }
            

            
            
            Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<Enemy>().Health);
            if (enemy.Dead)
            {

                enemy.Die(damage.Type);
                if (!enemy.CountedForBerserk)
                {
                    currentCurrency += enemy.Worth; //implementare lettura valore nemico
                    enemy.CountedForBerserk = true;
                }
                

                if(enemy.Type == EnemyType.Dragon)
                {
                    currentSkulls += 1;
                }

                if(damage.Type != DamageType.Berserk)
                    kills++;

                if (kills >= berserk)
                {
                    Messenger.Broadcast(GameEvent.BERSERK_ON);
                    StartCoroutine(BerserkHandle());
                    kills = 0;
                }
                //kills++;
            }
        }

    }


    
    public void OnHandleFoodAttack()
    {
        //riduazione costante della stamina
        float amount = Time.deltaTime * 1; //possibile estendere con un danno connesso all'attaccante
        foodStamina -= amount;
    }

    IEnumerator BerserkHandle()
    {
        Debug.Log("Head Camera Activated");
        countDown = 15; //15 secondi
        while(countDown >= 0)
        {
            yield return new WaitForSeconds(1);
            countDown -= 1;
        }

        Messenger.Broadcast(GameEvent.BERSERK_OFF);
        Debug.Log("Head Camera Deactivated");
    }
}
