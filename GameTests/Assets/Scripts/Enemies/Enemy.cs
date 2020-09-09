using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using DamagePackage;

public enum EnemyType { Barbarian, Dragon, Monster }


public class Enemy : MonoBehaviour
{
	private const char NEW_LINE = '\n';
	private const char EQUALS = '=';

	[SerializeField] private EnemyType type;
	private float[] damegesMultipliers;
	[HideInInspector]
	private Dictionary<string, float> multiplierDict;
	private Camera cam;
	private Camera headCamera;
	private Camera currentCamera;
	public float startHealth;
	private JoystickCharacterState playerStatus;
	private Rigidbody rigidbody;
	private float health;
	private int worth;
	private bool deathCounted;
	public Image hbContainer;
	public Image healthBar;




	public bool DeathCounted
	{
		get
		{
			return deathCounted;
		}

		set
		{
			deathCounted = value;
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

	public Dictionary<string, float> DamagesMultiplierDic
	{

		get
		{
			return multiplierDict;
		}


	}

	public float[] DamagesMultipliers
	{

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

	public bool Dead
	{
		get
		{
			return health <= 0;
		}
		
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
		multiplierDict = new Dictionary<string, float>();

		string eT = type.ToString();
		string filePath = "File/" + eT + "DamageMultipliers";
		List<float> dM = new List<float>();

		TextAsset data = Resources.Load<TextAsset>(filePath);
		string[] lines = data.text.Split(NEW_LINE);

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			string[] token = line.Split(EQUALS);

			switch (token[0])
			{
				case "startHealt":
					startHealth = float.Parse(token[1], CultureInfo.InvariantCulture);
					health = startHealth;
					break;
				case "worth":
					worth = int.Parse(token[1], CultureInfo.InvariantCulture);
					break;
				default:
					Debug.Log(token[0]);
					multiplierDict.Add(token[0], float.Parse(token[1], CultureInfo.InvariantCulture));
					break;
			}
		}

		
		cam = GameObject.Find("FollowingCamera").GetComponent<Camera>();
		headCamera = GameObject.Find("HeadCamera").GetComponent<Camera>();
		health = startHealth;
		playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<JoystickCharacterState>();
		rigidbody = GetComponent<Rigidbody>();


	}

    

	
    void OnCollisionEnter(Collision collision)
    {
        if(!playerStatus.IsBerserkOn && collision.gameObject.tag == "Player")
        {
			Debug.Log("HIT BY PLAYER");
			rigidbody.isKinematic = true;
        }
    }
    void OnCollisionStay(Collision collision)
    {
		if (!playerStatus.IsBerserkOn && !rigidbody.isKinematic)
			rigidbody.isKinematic = true;
    }
    void OnCollisionExit(Collision collision)
    {
		rigidbody.isKinematic = false;
	}
    public void Die(DamageType type)
	{
		
		float delay = 0;
		if (type == DamageType.Berserk)
		{
			delay = 2.0f;
		}

		Destroy(gameObject, delay);
	}

	void LateUpdate()
	{
		if (cam.enabled)
			hbContainer.transform.LookAt(healthBar.transform.position + cam.transform.forward);
		else
			hbContainer.transform.LookAt(healthBar.transform.position + headCamera.transform.forward);
	}
}
