using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{

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

	//Temporary added for Automatic Shooting character
	protected JoystickCharacterState status;
	Animator anim;


	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();    //Temporary added for Automatic Shooting character
		status = GetComponent<JoystickCharacterState>();    //Temporary added for Automatic Shooting character
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
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

	// Update is called once per frame
	void Update()
	{
		if (target == null)
		{
			anim.SetBool("isShooting", false);
			return;
		}
		//Temporary added for Shooting character
		if (!status.IsMoving && !status.IsRotating) //Temporary added for Automatic Shooting character
		{
			anim.SetBool("isShooting", true); //Temporary added for Automatic Shooting character
			LockOnTarget();

			if (fireCountdown <= 0f)
			{
				Shoot();
				bulletFired += 1;
				Debug.Log("Bullet count " + bulletFired);
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
		} else
		{
			anim.SetBool("isShooting", false); //Temporary added for Automatic Shooting character
		}
			

	}

	void LockOnTarget()
	{
		anim.SetBool("isShooting", true);
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Shoot()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null)
			bullet.Seek(target);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}