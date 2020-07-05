using UnityEngine;
using System.Collections;
using System.IO;

// Script attached to a Turret
public class Turret : Features
{

	private Transform target;
	public float range;
	public GameObject bulletPrefab;
	public float fireRate;
	private float fireCountdown = 0f;

	private int bulletFired = 0;

	public string enemyTag = "Enemy";

	public Transform partToRotate;
	public float turnSpeed ;

	public Transform firePoint;

	// Use this for initialization
	protected override void Start()
	{
		
		
		string path = "Assets/Scripts/Defense/turretFeatures.txt";
		StreamReader sr = new StreamReader(path);

		range = float.Parse(sr.ReadLine());
		fireRate = float.Parse(sr.ReadLine());
		turnSpeed = float.Parse(sr.ReadLine());
		//cost = int.Parse(sr.ReadLine());

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

	void LockOnTarget()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Shoot()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet1 bullet = bulletGO.GetComponent<Bullet1>();

		if (bullet != null)
			bullet.Seek(target);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}