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

    private float range = 15f;

    private GameObject weapon;
    private GameObject bulletPrefab;
    private float fireRate = 1f;
    private float fireCountdown = 0f;

    private int bulletFired = 0;

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    private int weaponSelected;
    private Transform firePoint;
    private GameObject rHand;
    private Vector3 handOffset = new Vector3(0.15f, 0.013f, 0.05f);




    void Start()
    {
        //default pari a 2 per testing
        weaponSelected = PlayerPrefs.GetInt("weaponSelected", 2);

        weapon = weaponPrefabs[weaponSelected - 1];
        bulletPrefab = bulletPrefabs[weaponSelected - 1];

        anim = GetComponent<Animator>();
        status = GetComponent<JoystickCharacterState>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

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
                default:
                    break;

            }
        }
        //range = float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);
        //fireRate = float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);

        rHand = GameObject.Find("rHand");
        weapon = Instantiate(weapon, rHand.transform.position, Quaternion.identity);
        weapon.transform.parent = rHand.transform;
        weapon.transform.localPosition = handOffset;
        weapon.transform.localRotation = Quaternion.Euler(-90, 0, 180);
        weapon.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        weapon.SetActive(true);

        firePoint = weapon.transform.Find("Pistol05_Imp02").gameObject.transform.Find("FirePoint");
        if (firePoint != null) Debug.Log("FIREPOINT TROVATO");


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

        //per risolvere il problema dell'angolazione penso bisogni aggiungere un ulteriore 
        Vector3 dir = target.position - transform.position;
        //Vector3 dir = target.position - firePoint.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        //Vector3 diff = partToRotate.forward - GameObject.Find("rForearmBend").transform.forward;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        //Vector3 diff = partToRotate.forward - GameObject.Find("rForearmBend").transform.forward;
        //Debug.Log("DIFFERENZA: " + diff);


    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet1 bullet = bulletGO.GetComponent<Bullet1>();

        if (bullet != null)
            bullet.Seek(target);
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
