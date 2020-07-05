 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DamagePackage;
using System;

public class LandMine : Features
{

    public GameObject explosionPrefab;
    private Vector3 explosionPosition;
    
    private _Damage damage;
    private float explosionRadius = 10.0f;
    private float explosionPower = 1000.0f;
    private bool explosion = false;

    // Start is called before the first frame update
    void Awake()
    {
        string path = "Assets/Scripts/Defense/landMineFeatures.txt";
        StreamReader sr = new StreamReader(path);

        DamageType type = (DamageType)Enum.Parse(typeof(DamageType), sr.ReadLine());
        string descr = sr.ReadLine();
        float amount = float.Parse(sr.ReadLine());

        damage = new _Damage(type, descr, amount);
        explosionRadius = float.Parse(sr.ReadLine());
        explosionPower = float.Parse(sr.ReadLine());
        //cost = int.Parse(sr.ReadLine());

        explosionPosition = transform.position;
        explosionPrefab = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosionPrefab.SetActive(false);

        //Debug.Log("Tranform Mina: " + transform.position);
        //Debug.Log("Tranform Esplosione: " + explosionPrefab.transform.position);
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
                    //Messenger<GameObject,int>.Broadcast(GameEvent.ENEMY_HIT, hit.gameObject,damage);
                    Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, hit.gameObject, damage);
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
                        //Messenger<GameObject,int>.Broadcast(GameEvent.ENEMY_HIT, hit.gameObject,damage);
                        Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, hit.gameObject, damage);
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