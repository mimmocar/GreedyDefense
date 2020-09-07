using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;
using UnityEngine.AI;
using System.Globalization;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    //Start is called before the first frame update
    private static ObjectManager _instance;
    private int startCurrency; //implementare lettura da file/PlayerPrefs
    private int currentCurrency;
    private int startSkulls; //implementare lettura da file/PlayerPrefs
    private int currentSkulls;
    private int hit = 0, kills = 0, berserk = 100;  //inizializzazione all'inizio del livello
    [SerializeField] GameObject[] prefab;
    [SerializeField] protected GameObject enterEffect;
    private float timeEffect;

    private int berserkCountdown;
    private GameControl gameControl;
    private float startFoodStamina; //valore di inizializzazione costante
    private float foodStamina;
    private int countDown;
    private float explosionPower;
    private float explosionRadius;
    private float upwardMod;
    private float enemyOffset;
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


    public static ObjectManager Instance(){

        if (_instance == null)
            _instance = FindObjectOfType<ObjectManager>();
        return _instance;
    }

    void Awake()
    {
        //startSkulls = PlayerPrefs.GetInt("skullsCurrency", 0);
        //currentCurrency = startCurrency;
        //foodStamina = startFoodStamina;
        //currentSkulls = startSkulls;   //per il momento lasciamo anche startSkulls, a seconda di come valutare il punteggio
        

        for (int i = 0; i < prefab.Length; i++)
        {
            prefab[i].GetComponent<Features>().Awake();
        }

        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.AddListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
        Messenger<float>.AddListener(GameEvent.HANDLE_FOOD_ATTACK, OnHandleFoodAttack);
        Debug.Log("LETTURA FEATURES COST "+prefab[0].GetComponent<Features>().Cost);

        gameControl = GameControl.Instance();


        string filePath = "File/Level" + gameControl.currentLevel + "Features";
        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "startCurrency":
                    startCurrency = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "startFoodStamina":
                    startFoodStamina = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "explosionPower":
                    explosionPower = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "explosionRadius":
                    explosionRadius = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "upwardMod":
                    upwardMod = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "enemyOffset":
                    enemyOffset = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "berserk":
                    berserk = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "berserkCountdown":
                    berserkCountdown = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }
        }

        startSkulls = PlayerPrefs.GetInt("skullsCurrency", 0);
        currentCurrency = startCurrency;
        foodStamina = startFoodStamina;
        currentSkulls = startSkulls;

        //Aggiunto per motivi di testing
        kills = 0;
    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, _Damage>.RemoveListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
    }

    private void Start()
    {
        //inizializza lettura monete, vita del cibo, raggio dell'esplosione, potenza dell'esplosione, countdown bererk, upward modification Anche per LandMine
        // anche enemyOffset
        //gameControl = GameControl.Instance();


        //string filePath = "File/Level"+gameControl.currentLevel+"Features";
        //TextAsset data = Resources.Load<TextAsset>(filePath);
        //string[] lines = data.text.Split('\n');

        //for (int i = 0; i < lines.Length; i++)
        //{
        //    string line = lines[i];
        //    string[] token = line.Split('=');

        //    switch (token[0])
        //    {
        //        case "startCurrency":
        //            startCurrency = int.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "startFoodStamina":
        //            startFoodStamina = float.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "explosionPower":
        //            explosionPower = float.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "explosionRadius":
        //            explosionRadius = float.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "upwardMod":
        //            upwardMod = float.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "enemyOffset":
        //            enemyOffset = float.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "berserk":
        //            berserk = int.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        case "berserkCountdown":
        //            berserkCountdown = int.Parse(token[1], CultureInfo.InvariantCulture);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //startSkulls = PlayerPrefs.GetInt("skullsCurrency", 0);
        //currentCurrency = startCurrency;
        //foodStamina = startFoodStamina;
        //currentSkulls = startSkulls;

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

                target.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, target.transform.position, explosionRadius, upwardMod, ForceMode.Force); //implementare lettura da file
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
                if (!enemy.DeathCounted)
                {
                    currentCurrency += enemy.Worth; //implementare lettura valore nemico

                    if (enemy.Type == EnemyType.Dragon)
                    {
                        currentSkulls += 1;
                    }

                    if (damage.Type != DamageType.Berserk)
                        kills++;
                }
                


                if (kills >= berserk)
                {
                    Messenger.Broadcast(GameEvent.BERSERK_ON);
                    StartCoroutine(BerserkHandle());
                    kills = 0;
                }

                if (!enemy.DeathCounted)
                {
                    enemy.DeathCounted = true;
                    StartCoroutine(Die(enemy.gameObject));
                }
                    
                //kills++;
            }
        }

    }


    IEnumerator Die(GameObject enemy)
    {
        
            enemy.GetComponent<NavMeshAgent>().enabled = false;
            enemy.GetComponent<MoveDestination>().enabled = false;
            Animator anim = enemy.GetComponent<Animator>();
            anim.SetBool("isDead", true);

            var animController = anim.runtimeAnimatorController;
            var clip = animController.animationClips[1];

            yield return new WaitForSeconds(clip.length);

            if(enemy != null)
                enemy.transform.position = new Vector3(enemyOffset, enemyOffset, enemyOffset);
            yield return new WaitForEndOfFrame();


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
        countDown = berserkCountdown; //15 secondi
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
