using UnityEngine;
using System.Globalization;
using System.IO;
using System;

using DamagePackage;

// Script placed to a Bullet
public enum BulletType {bullet1,bullet2, missile }

public class Bullet1 : MonoBehaviour
{
	[SerializeField] private BulletType type;
	private Transform target;
	public float speed = 70f;
	public GameObject impactEffect;
	private _Damage damage;

    private void Awake()
    {
		
		Debug.Log(type.ToString());
		damage = _Damage.ReadDamage("Assets/Resources/File/"+type.ToString()+"Features.txt");
    }


    public void Seek(Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update()
	{

		if (target == null)
		{
			Destroy(gameObject);
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
		GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectIns, 5f);

		Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, target.gameObject, damage);
		
		Destroy(gameObject);
	}

    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		
	}
}
