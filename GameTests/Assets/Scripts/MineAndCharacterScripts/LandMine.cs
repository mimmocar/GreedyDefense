 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{

    public GameObject explosionPrefab;
    private Vector3 explosionPosition;
    private float explosionRadius = 50.0f;
    private float explosionPower = 100000.0f;
    // Start is called before the first frame update
    void Start()
    {
        explosionPosition = transform.position;
        explosionPrefab = Instantiate(explosionPrefab, transform.position,transform.rotation);
        explosionPrefab.SetActive(false);

        Debug.Log("Tranform Mina: " + transform.position);
        Debug.Log("Tranform Esplosione: " + explosionPrefab.transform.position);    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider other)
    {

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>())
            { // if it's a rigidbody, add explosion force:
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 3.0f);
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
                }

            }
        }
        
        transform.gameObject.SetActive(false);
        Destroy(this);
        //gameObject.SetActive(false);

    }
}
