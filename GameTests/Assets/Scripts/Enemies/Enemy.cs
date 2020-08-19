using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using DamagePackage;

public enum EnemyType { Barbarian, Dragon, Monster}


public class Enemy : MonoBehaviour 
{
		
		[SerializeField] private EnemyType type;
		private float[] damegesMultipliers;
		[HideInInspector]

		private Camera cam;
		private Camera headCamera;
		private Camera currentCamera;
		public float startHealth;
		private float health;
		private int worth;
		private bool countedForBerserk;


	private int enemiesDied = 0;


	public bool CountedForBerserk
    {
        get
        {
			return countedForBerserk;
        }

        set
        {
			countedForBerserk = value;
        }
    }
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

	public EnemyType Type
    {
        get
        {
			return type;
        }
    }


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


	[Header("Unity Stuff")]
	public Image hbContainer;
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

		startHealth = float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);
		health = startHealth;
		worth = int.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);

        while (!sr.EndOfStream)
        {
			dM.Add(float.Parse(sr.ReadLine(), CultureInfo.InvariantCulture));
        }

		damegesMultipliers = dM.ToArray();
        for(int i=0;i< damegesMultipliers.Length; i++)
        {
			Debug.Log(i + "   " + damegesMultipliers[i]);

        }


		cam = Camera.main;
		headCamera = GameObject.Find("HeadCamera").GetComponent<Camera>();
		health = startHealth;



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

	public void Die(DamageType type)
	{
		//isDead = true;

		// PlayerStats.Money += worth;

		// GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		// Destroy(effect, 5f);

		//WaveSpawner1.EnemiesAlive--;
		//Died++;

		//Messenger<int>.Broadcast("Enemy died", Died);
		float delay = 0;
		if(type == DamageType.Berserk)
        {
			delay = 2.0f;
		}
        
		Destroy(gameObject, delay);
	}

	void LateUpdate()
    {
		if(cam.enabled)
			hbContainer.transform.LookAt(healthBar.transform.position + cam.transform.forward);
		else
			hbContainer.transform.LookAt(healthBar.transform.position + headCamera.transform.forward);
	}
}
