using UnityEngine;
using System.Globalization;
using System.IO;
using System;

using DamagePackage;
using System.Collections;

// Script placed to a Bullet
public enum BulletType { bullet1, bullet2, missile }

public class Bullet1 : PoolObject
{
	[SerializeField] private BulletType type;
	private Transform target;
	private float speed;
	public GameObject impactEffect;
	private GameObject effectIns;
	private _Damage damage;

	private void Awake()
	{

		Debug.Log(type.ToString());
		damage = _Damage.ReadDamage("File/" + type.ToString() + "Features");//riaggiustare  mettendo la lettura della speed;
		string filePath = "File/" + type.ToString() + "Features";
		
		TextAsset data = Resources.Load<TextAsset>(filePath);
		string[] lines = data.text.Split('\n');

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			string[] token = line.Split('=');

			switch (token[0])
			{
				case "speed":
					speed = float.Parse(token[1], CultureInfo.InvariantCulture);
					break;
				default:
					break;
			}
		}
	}

    void OnEnable()
    {
		Debug.Log("JUST ACTIVATED");
    }

    void Start()
    {
		GameObject poolHolder = GameObject.Find("PoolManager");
		effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		effectIns.transform.parent = poolHolder.transform;
		effectIns.SetActive(false);
	}


    public override void OnObjectReuse(Transform target)
    {
        base.OnObjectReuse(target);
		Seek(target);
    }

    private void Seek(Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update()
	{

		if (target == null)
		{
			
			Destroy();
			return;
		}

		Vector3 dir = target.position - transform.position;
		dir.y += target.transform.localScale.y / 2;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);

	}

	private void ActiveEffect()
    {

		effectIns.transform.position = transform.position;
		effectIns.transform.rotation = transform.rotation;
		effectIns.SetActive(true);
		Animation anim;
		ParticleSystem ps = effectIns.GetComponent<ParticleSystem>();
		if (ps != null) ps.Play();
		if (effectIns.GetComponent<Animation>() != null)
		{
			Debug.Log("ANIM FOUND");
			anim = effectIns.GetComponent<Animation>();
			anim.Play();
		}
		Debug.Log("EFFETTO ATTIVO " + effectIns.transform.position);


		StartCoroutine(DestroyEffect());
	}

    private void OnTriggerEnter(Collider other)
    {
		
		if (other.gameObject.tag != "Enemy")
		{
			ActiveEffect();
			Debug.Log("BULLET DESTROYED because of: "+ other.gameObject.name);
			Destroy();
	     }
	}
	

	void HitTarget()
	{

		
		ActiveEffect();

		Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, target.gameObject, damage);

		Destroy();
		
	}

	IEnumerator  DestroyEffect()
    {
		yield return new WaitForSeconds(5.0f);
		
		effectIns.SetActive(false);
		Debug.Log("EFFETTO DISATTIVO " + effectIns.transform.position);
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

	}
}
