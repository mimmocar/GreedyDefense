using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public GameObject character;
    protected JoystickCharacterState status;
    Animator anim;

    private Transform target;

    public float range = 15f;

    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    private int bulletFired = 0;

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;


    private bool possibleShooting;

    void Start()
    {
        anim = character.GetComponent<Animator>();
        status = character.GetComponent<JoystickCharacterState>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        possibleShooting = false;
    }
    void Awake()
    {
        Messenger.AddListener(GameEvent.SHOOTING, OnShooting);
        Messenger.AddListener(GameEvent.STOP_SHOOTING, OnStopShooting);
        
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SHOOTING, OnShooting);
        Messenger.RemoveListener(GameEvent.STOP_SHOOTING, OnStopShooting);
    }

    void OnShooting()
    {
        anim.SetBool("isShooting", true);
        possibleShooting = true;

    }
    void OnStopShooting()
    {
        anim.SetBool("isShooting", false);
        possibleShooting = false;
    }


    void Update()
    {
        if(possibleShooting) 
        {
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
        
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - character.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet1 bullet = bulletGO.GetComponent<Bullet1>();

        //if (bullet != null)
        //    bullet.Seek(target);
    }

    void UpdateTarget()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(character.transform.position, enemy.transform.position);
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
		Gizmos.DrawWireSphere(character.transform.position, range);
	}

}
