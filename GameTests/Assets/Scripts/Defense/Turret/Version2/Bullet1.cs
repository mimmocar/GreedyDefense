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
	public float speed = 70f;
	public GameObject impactEffect;
	private GameObject effectIns;
	private _Damage damage;

	private void Awake()
	{

		Debug.Log(type.ToString());
		damage = _Damage.ReadDamage("File/" + type.ToString() + "Features");
	}

    void Start()
    {
		effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		//effectIns.transform.parent = transform.parent;
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
			//Destroy(gameObject);
			Destroy();
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);

	}

	void HitTarget()
	{
		//GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		effectIns.transform.position = transform.position;
		effectIns.transform.rotation = transform.rotation;
		effectIns.SetActive(true);
		Animation anim;
		ParticleSystem ps = effectIns.GetComponent<ParticleSystem>() ;
		if (ps != null) ps.Play();
        if (effectIns.GetComponent<Animation>() != null)
        {
            Debug.Log("ANIM FOUND");
            anim = effectIns.GetComponent<Animation>();
            anim.Play();
        }
        Debug.Log("EFFETTO ATTIVO " + effectIns.transform.position);

		//Destroy(effectIns, 5f);
		StartCoroutine(DestroyEffect());

		Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, target.gameObject, damage);

		Destroy();
		//Destroy(gameObject);
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
