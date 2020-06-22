using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{

    public Rigidbody bulletPrefab;
    private Transform target;
    private GameObject bullet;
    private float nextFire;
    private float rotationSpeed = 5f;
    private Quaternion targetPos;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected float deltaFire = 0.5f;

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Enemy"))
        {
            Debug.Log("in");
            target = otherCollider.transform;
            StartCoroutine("Fire");
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Enemy"))
        {
            Debug.Log("out");
            target = null;
            StopCoroutine("Fire"); // aborts the currently running Fire() coroutine
        }
    }

    IEnumerator Fire()
    {
        while (target != null)
        {
            nextFire = Time.time + deltaFire;
            while (Time.time < nextFire)
            {
                // smooth the moving of the turret
                Vector3 dir = target.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return new WaitForEndOfFrame();
            }
            // fire!
            Debug.Log("shoot");
            Rigidbody p = Instantiate(bulletPrefab, transform.position, transform.rotation);
            p.velocity = transform.forward * bulletSpeed;
        }
    }
}
