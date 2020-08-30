using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;

public class AutoShooting : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    //public GameObject character;
    protected JoystickCharacterState status;
    Animator anim;


    public GameObject[] weaponPrefabs;
    public GameObject[] bulletPrefabs;
    private Transform target;
    private PoolManager poolManager;
    private float range;

    private GameObject weapon;
    private GameObject bulletPrefab;
    private float fireRate;
    private float fireCountdown = 0f;

    private int bulletFired = 0;

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed;

    private int weaponSelected;
    private Transform firePoint;
    private GameObject rHand;
    //private Vector3 handOffset = new Vector3(0.15f, 0.013f, 0.05f);
    //private Quaternion weaponRotation;

    
    private float startShootingTime;
    private float repeatShootingTime;
    private int bulletPoolSize;
    //private float scaleWeapon;

    void Start()
    {
        //default pari a 2 per testing
        weaponSelected = PlayerPrefs.GetInt("weaponSelected", 1);

        weapon = weaponPrefabs[weaponSelected - 1];
        bulletPrefab = bulletPrefabs[weaponSelected - 1];

        anim = GetComponent<Animator>();
        status = GetComponent<JoystickCharacterState>();
      //  InvokeRepeating("UpdateTarget", 0f, 0.5f); //nuovi parametri sono startShooting  time e repeatShootingTime

        string filePath = "File/weapon" + weaponSelected + "Features";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "range":
                    range = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "fireRate":
                    fireRate = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:   //aggiungere alla lettura  e handOffset, scaleWeapon e rotation params, bulletPoolSize  
                    break;

            }
        }

        //Vector3 handOffset = new Vector3(0.15f, 0.013f, 0.05f);
        Vector3 handOffset;
        Quaternion weaponRotation;

        rHand = GameObject.Find("rHand");
        weapon = Instantiate(weapon, rHand.transform.position, Quaternion.identity);
        weapon.transform.parent = rHand.transform;

        filePath = "File/autoshootingFeatures";
        data = Resources.Load<TextAsset>(filePath);
        lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "turnSpeed":
                    turnSpeed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "startShootingTime":
                    startShootingTime = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "repeatShootingTime":
                    repeatShootingTime = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "bulletPoolSize":
                    bulletPoolSize = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "scaleWeapon":
                    float scaleWeapon  = float.Parse(token[1], CultureInfo.InvariantCulture);
                    weapon.transform.localScale = new Vector3(scaleWeapon, scaleWeapon, scaleWeapon);
                    break;
                case "handOffset":
                    string[] componentsV = token[1].Split(',');
                    float xV = float.Parse(componentsV[0], CultureInfo.InvariantCulture);
                    float yV = float.Parse(componentsV[1], CultureInfo.InvariantCulture);
                    float zV = float.Parse(componentsV[2], CultureInfo.InvariantCulture);
                    handOffset = new Vector3(xV, yV, zV);
                    weapon.transform.localPosition = handOffset;
                    break;
                case "weaponRotation":
                    string[] componentsQ = token[1].Split(',');
                    float xQ = float.Parse(componentsQ[0], CultureInfo.InvariantCulture);
                    float yQ = float.Parse(componentsQ[1], CultureInfo.InvariantCulture);
                    float zQ = float.Parse(componentsQ[2], CultureInfo.InvariantCulture);
                    weaponRotation = Quaternion.Euler(xQ, yQ, zQ);
                    weapon.transform.localRotation = weaponRotation;
                    break;

                default:   //aggiungere alla lettura turnSpeed e handOffset, scaleWeapon e rotation params, bulletPoolSize  
                    break;

            }
        }

        

        weapon.SetActive(true);

        firePoint = weapon.transform.Find("Pistol05_Imp02").gameObject.transform.Find("FirePoint");
        if (firePoint != null) Debug.Log("FIREPOINT TROVATO");


        poolManager = GetComponent<PoolManager>();
        poolManager.CreatePool(bulletPrefab, bulletPoolSize); // da leggere da file

        InvokeRepeating("UpdateTarget", startShootingTime, repeatShootingTime); //nuovi parametri sono startShooting  time e repeatShootingTime
    }



    void Update()
    {
        if (status.IsShooting)
        {
            anim.SetBool("isShooting", true);
            if (target == null)
            {
                return;
            }
            LockOnTarget();

            if (fireCountdown <= 0f)
            {
                Shoot();
                bulletFired += 1;
                Debug.Log("Bullet count " + bulletFired);
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;

        }
        else
        {
            anim.SetBool("isShooting", false);
        }

    }

    void LockOnTarget()
    {
        
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }

    void Shoot()
    {        
        poolManager.ReuseObject(bulletPrefab, firePoint.position, firePoint.rotation, target);
    }

    void UpdateTarget()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
