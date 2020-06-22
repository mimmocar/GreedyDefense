using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is place on a projectile GameObject
public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public GameObject explosion;

    //void Start()
    //{
    //    // The projectile is deleted after 10 seconds, whether
    //    // or not it collided with anything (to prevent lost
    //    // instances sticking around in the scene forever)
    //    Destroy(gameObject, 10);
    //}
  

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }

    //void OnCollisionEnter()
    //{
    //    // it hit something: create an explosion, and remove the projectile
    //    Instantiate(explosion, transform.position, transform.rotation);
    //    Destroy(gameObject);
    //}
}
