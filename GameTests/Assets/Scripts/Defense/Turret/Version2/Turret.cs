using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;

public enum TurretType { standard, missile };
// Script attached to a Turret
public class Turret : Features
{
	private const char NEW_LINE = '\n';
	private const char EQUALS = '=';

	public TurretType turretType;
	private Transform target;
	private float range;
	public GameObject bulletPrefab;
	private float fireRate;
	private float fireCountdown = 0f;

	private int bulletFired = 0;

	public string enemyTag = "Enemy";

	public Transform partToRotate;
	private float turnSpeed;

	public Transform firePoint;

	// Use this for initialization
	public override void Awake()
	{
		string filePath = "File/" + turretType.ToString() + "turretFeatures";

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
				case "turnSpeed":
					turnSpeed = float.Parse(token[1], CultureInfo.InvariantCulture);
					break;
				case "cost":
					cost = int.Parse(token[1], CultureInfo.InvariantCulture);
					Debug.Log("COSTO ASSET LETTO: " + cost);
					break;
				default:
					break;
			}
		}

		//range = float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);
		//fireRate = float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);
		//turnSpeed = float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);
		//cost = int.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);

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