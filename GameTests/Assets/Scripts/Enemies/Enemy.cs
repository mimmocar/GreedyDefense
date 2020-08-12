using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public enum EnemyType { Barbarian, Dragon, Monster}


public class Enemy : MonoBehaviour 
{
		
<<<<<<< HEAD
		[SerializeField] private EnemyType type;
		private float[] damegesMultipliers;
	[HideInInspector]

		private Camera cam;
		public float startHealth;
		private float health;
		private int worth;
=======
	[SerializeField] private EnemyType type;
	private float[] damegesMultipliers;
	[HideInInspector]
		
	public float startHealth;
	private float health;
	private int worth;

	private int enemiesDied = 0;

	public int Died
	{
		get
		{
			return enemiesDied;
		}

		set
		{
			enemiesDied = value;
		}
	}

>>>>>>> ae71a95e0d1272633363b42e3feaef3c68a0965b

    // public GameObject deathEffect;

	public float[] DamagesMultipliers {

		get
		{
		  return damegesMultipliers;
		}


	}

	public int Worth
    {
        get
        {
			return worth;
        }
    }

<<<<<<< HEAD
	[Header("Unity Stuff")]
	public Image hbContainer;
=======
>>>>>>> ae71a95e0d1272633363b42e3feaef3c68a0965b
	public Image healthBar;

	private bool isDead = false;
	public bool Dead
	{
		get
		{
			return health <= 0;
		}
			//set
			//{
			//	isDead = value;
			//}
	}

	public float Health
	{
		get
		{

			return health;
		}

		set
		{
			health = value;
		}
	}

	void Start()
	{
		
		
		string eT = type.ToString();
		string path = "Assets/Resources/File/" + eT + "DamageMultipliers.txt";
		Debug.Log("PATH LETTO PER IL NEMICO "+eT+": "+path);
		StreamReader sr = new StreamReader(path);
	    List<float> dM = new List<float>();

		startHealth = float.Parse(sr.ReadLine());
		health = startHealth;
		worth = int.Parse(sr.ReadLine());

        while (!sr.EndOfStream)
        {
			dM.Add(float.Parse(sr.ReadLine()));
        }

		damegesMultipliers = dM.ToArray();
        for(int i=0;i< damegesMultipliers.Length -1; i++)
        {
			Debug.Log(i + "   " + damegesMultipliers[i]);

        }

<<<<<<< HEAD
		cam = Camera.main;
=======
		health = startHealth;
>>>>>>> ae71a95e0d1272633363b42e3feaef3c68a0965b


	}

		//public void TakeDamage(float amount)
		//{
		//	health -= amount;

		//	//healthBar.fillAmount = health / startHealth;

		//	if (health <= 0 && !isDead)
		//	{
		//		isDead = true;
		//		//Die();
		//	}
		//}

		//public void Slow (float pct)
		//{
		//	speed = startSpeed * (1f - pct);
		//}

	public void Die()
	{
		//isDead = true;

		// PlayerStats.Money += worth;

		// GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		// Destroy(effect, 5f);

		//WaveSpawner1.EnemiesAlive--;
		//Died++;

		//Messenger<int>.Broadcast("Enemy died", Died);
		Destroy(gameObject);
	}

	void LateUpdate()
    {
		hbContainer.transform.LookAt(healthBar.transform.position + cam.transform.forward);
    }
}
