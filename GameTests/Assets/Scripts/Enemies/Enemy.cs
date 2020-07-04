using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public enum EnemyType { Barbarian, Dragon, Monster}


public class Enemy : MonoBehaviour {

		
		[SerializeField] private EnemyType type;
		private float[] damegesMultipliers;
		[HideInInspector]
		
		public float startHealth = 100;
		private float health;

    // public GameObject deathEffect;

	public float[] DamagesMultipliers {

		get
		{
		  return damegesMultipliers;
		}


	}

	[Header("Unity Stuff")]
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
		
		health = startHealth;
		string eT = type.ToString();
		string path = "Assets/Scripts/Enemies/" + eT + "DamageMultipliers.txt";
		Debug.Log("PATH LETTO PER IL NEMICO "+eT+": "+path);
		StreamReader sr = new StreamReader(path);
	    List<float> dM = new List<float>();

        while (!sr.EndOfStream)
        {
			dM.Add(float.Parse(sr.ReadLine()));
        }

		damegesMultipliers = dM.ToArray();
        for(int i=0;i< damegesMultipliers.Length -1; i++)
        {
			Debug.Log(i + "   " + damegesMultipliers[i]);

        }


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

			// WaveSpawner.EnemiesAlive--;

	Destroy(gameObject);
	}

}
