 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{

    public GameObject explosionPrefab;
    private Vector3 explosionPosition;
    private int damage = 100;
    private float explosionRadius = 10.0f;
    private float explosionPower = 1000.0f;
    private bool explosion = false;
    // Start is called before the first frame update
    void Start()
    {
        explosionPosition = transform.position;
        explosionPrefab = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosionPrefab.SetActive(false);

        //Debug.Log("Tranform Mina: " + transform.position);
        //Debug.Log("Tranform Esplosione: " + explosionPrefab.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Enemy")
        {
            Debug.Log(other.tag);
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
            foreach (Collider hit in colliders)
            {

                if (hit.tag != "Enemy") continue;

                Debug.Log(hit.tag);

                //hit.GetComponent<Animator>().enabled = false;
                //hit.GetComponent<MoveDestination>().enabled = false;
                if (hit.GetComponent<Rigidbody>())
                { // if it's a rigidbody, add explosion force:
                    Debug.Log("Explosion with RigidBody");
                    explosionPrefab.SetActive(true);
                    hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 0.0f, ForceMode.Force);
                    Messenger<GameObject,int>.Broadcast(GameEvent.ENEMY_HIT, hit.gameObject,damage);
                    Debug.Log(hit);
                    explosion = true;
                }
                else
                { // but if it's a character with ImpactReceiver, add the impact:
                    ImpactReceiver script = hit.GetComponent<ImpactReceiver>();
                    if (script)
                    {

                        Debug.Log("Explosion ImpactReceiver");
                        explosionPrefab.SetActive(true);

                        Vector3 dir = hit.transform.position - explosionPosition;
                        float force = Mathf.Clamp(explosionPower / 3, 0, 5000);
                        script.AddImpact(dir, force);
                        Messenger<GameObject,int>.Broadcast(GameEvent.ENEMY_HIT, hit.gameObject,damage);
                        explosion = true;
                    }

                }

                //hit.GetComponent<Animator>().enabled = true;
                //hit.GetComponent<MoveDestination>().enabled = true;
            }
        }
        if (explosion)
        {
            transform.gameObject.SetActive(false);
            Destroy(this);
            //gameObject.SetActive(false);
        }

    }
}