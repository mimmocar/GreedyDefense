using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;
using UnityEngine.AI;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    //Start is called before the first frame update

    private int startCurrency = 70; //implementare lettura da file/PlayerPrefs
    private int currentCurrency;
    private int startSkulls; //implementare lettura da file/PlayerPrefs
    private int currentSkulls;
    private int hit = 0, kills = 0, berserk = 100;  //inizializzazione all'inizio del livello
    [SerializeField] GameObject[] prefab;
    [SerializeField] protected GameObject enterEffect;
    private float timeEffect;

    private float startFoodStamina = 10; //valore di inizializzazione costante
    private float foodStamina;
    private int countDown;

    private float waveCountdown;
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

    public int Skulls
    {
        get
        {
            return currentSkulls;
        }
    }


    void Awake()
    {
        startSkulls = PlayerPrefs.GetInt("skullsCurrency", 0);
        currentCurrency = startCurrency;
        foodStamina = startFoodStamina;
        currentSkulls = startSkulls;   //per il momento lasciamo anche startSkulls, a seconda di come valutare il punteggio


        for (int i = 0; i < prefab.Length; i++)
        {
            prefab[i].GetComponent<Features>().Awake();
        }

        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.AddListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
        Messenger<float>.AddListener(GameEvent.HANDLE_FOOD_ATTACK, OnHandleFoodAttack);
        Debug.Log("LETTURA FEATURES COST "+prefab[0].GetComponent<Features>().Cost);


        //Aggiunto per motivi di testing
        kills = 90;
    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.RemoveListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
    }

    private void Start()
    {
        
    }

    

    public void OnSpawnObject(Vector3 position, int i)
    {
        float cost = prefab[i].GetComponent<Features>().Cost;
        if (currentCurrency >= cost)
        {
            //position += new Vector3(0, prefab[i].transform.localScale.y / 2, 0);
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
                
                Dictionary<string, float> multipliers = enemy.DamagesMultiplierDic;
                float amount = damage.Amount;
                
                string key = damage.Type.ToString() + "Multiplier";


                enemy.Health = enemy.Health - multipliers[key] * amount;
            }
            

            
            
            Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<Enemy>().Health);
            if (enemy.Dead)
            {

                //enemy.Die(damage.Type);
                //StartCoroutine(Die(enemy));
                if (!enemy.CountedForBerserk)
                {
                    currentCurrency += enemy.Worth; //implementare lettura valore nemico
                    enemy.CountedForBerserk = true;

                    if (enemy.Type == EnemyType.Dragon)
                    {
                        currentSkulls += 1;
                    }
                }
                

                //if(enemy.Type == EnemyType.Dragon)
                //{
                //    currentSkulls += 1;
                //}

                if(damage.Type != DamageType.Berserk)
                    kills++;

                if (kills >= berserk)
                {
                    Messenger.Broadcast(GameEvent.BERSERK_ON);
                    StartCoroutine(BerserkHandle());
                    kills = 0;
                }

                StartCoroutine(Die(enemy.gameObject));
                //kills++;
            }
        }

    }


    IEnumerator Die(GameObject enemy)
    {
        enemy.GetComponent<NavMeshAgent>().enabled=false;
        enemy.GetComponent<MoveDestination>().enabled = false;
        Animator anim = enemy.GetComponent<Animator>();
        anim.SetBool("isDead",true);

        var animController = anim.runtimeAnimatorController;
        var clip = animController.animationClips[1];
        //while (anim.GetCurrentAnimatorStateInfo(0).IsName("BarbarianDie")){
        //    yield return new WaitForSeconds(Time.deltaTime);
        //}
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(clip.length);
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0). - anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Destroy(enemy);
    }
    public void OnHandleFoodAttack(float damageMult)
    {
        //riduazione costante della stamina
        float amount = Time.deltaTime * damageMult; //possibile estendere con un danno connesso all'attaccante
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

    public void Highliner(float time)
    {
        GameObject food = GameObject.FindGameObjectWithTag("food");
        timeEffect = time;
        GameObject effect = (GameObject)Instantiate(enterEffect, food.transform.position, food.transform.rotation);
        Destroy(effect, timeEffect);

    }
}
