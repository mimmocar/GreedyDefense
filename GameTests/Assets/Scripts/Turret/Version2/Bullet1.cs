using UnityEngine;
using DamagePackage;

// Script placed to a Bullet 
public class Bullet1 : MonoBehaviour
{

	private Transform target;
	public float speed = 70f;
	//public int damage = 50;
	public float explosionRadius = 0f;
	public GameObject impactEffect;
	private _Damage damage;

    private void Awake()
    {
		damage = _Damage.ReadDamage("Assets/Scripts/Turret/Version2/bulletFeatures.txt");
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

		if (explosionRadius > 0f)
		{
			Explode();
		}
		else
		{
			Damage(target);
			//Messenger<GameObject,int>.Broadcast(GameEvent.ENEMY_HIT, target.gameObject,damage);
			Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, target.gameObject, damage);
		}

		Destroy(gameObject);
	}

	void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
			}
		}
	}

    void Damage(Transform enemy)
    {
		Enemy e = enemy.GetComponent<Enemy>();

		if (e != null)
		{
			//Messenger<GameObject, int>.Broadcast(GameEvent.ENEMY_HIT, enemy.gameObject, damage);
			//    e.TakeDamage(damage);
		}
		
	}

    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
