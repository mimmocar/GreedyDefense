using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is placed on a projectile GameObject
// This script uses Message, intercepted by ObjectManager script
public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public GameObject explosion;
    private bool hit = false;
    void Start()
    //

    {


        explosion = Instantiate(explosion, transform.position, transform.rotation);
        explosion.SetActive(false);
        //    // The projectile is deleted after 10 seconds, whether
        //    // or not it collided with anything (to prevent lost
        //    // instances sticking around in the scene forever)
        //    Destroy(gameObject, 10);
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

            //Debug.Log("Enemy Hit");
            //Messenger<GameObject>.Broadcast(GameEvent.ENEMY_HIT, target.gameObject);
            //HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hit)
        {
            Debug.Log("Enemy Hit");

            explosion.transform.position = transform.position;

            Debug.Log("Explosion position: " + explosion.transform.position);
            Debug.Log("Hit position: " + other.transform.position);
            explosion.SetActive(true);
            Messenger<GameObject>.Broadcast(GameEvent.ENEMY_HIT, target.gameObject);
            hit = true;
            HitTarget();
        }



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
