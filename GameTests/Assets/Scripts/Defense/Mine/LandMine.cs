using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DamagePackage;
using System;
using System.Globalization;

public class LandMine : Features
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    public GameObject explosionPrefab;
    private Vector3 explosionPosition;

    private _Damage damage;
    private float explosionRadius;
    private float explosionPower;
    private float upwardMod;
    private bool explosion = false;

    // Start is called before the first frame update
    public override void Awake()
    {
        string filePath = "File/landMineFeatures";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        damage = new _Damage();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "type":
                    damage.Type = (DamageType)Enum.Parse(typeof(DamageType), token[1]);
                    break;
                case "description":
                    damage.Description = token[1];
                    break;
                case "amount":
                    damage.Amount = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "explosionRadius":
                    explosionRadius = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "explosionPower":
                    explosionPower = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "cost":
                    cost = int.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("COSTO ASSET LETTO: " + cost);
                    break;
                case "upwardMod":
                    upwardMod = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;

            }

        }

        
        

        
    }


    void Start()
    {
        explosionPosition = transform.position;
        explosionPrefab = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosionPrefab.SetActive(false);
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

                if (hit.GetComponent<Rigidbody>())
                { // if it's a rigidbody, add explosion force:
                    Debug.Log("Explosion with RigidBody");
                  
                    explosionPrefab.SetActive(true);
                    hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRadius, upwardMod, ForceMode.Force);

                    Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, hit.gameObject, damage);
                    Debug.Log(hit);

                    explosion = true;
                }
                //else
                //{ // but if it's a character with ImpactReceiver, add the impact:
                //    ImpactReceiver script = hit.GetComponent<ImpactReceiver>();
                //    if (script)
                //    {

                //        Debug.Log("Explosion ImpactReceiver");
                //        explosionPrefab.SetActive(true);

                //        Vector3 dir = hit.transform.position - explosionPosition;
                //        float force = Mathf.Clamp(explosionPower / 3, 0, 5000);
                //        script.AddImpact(dir, force);
                //        //Messenger<GameObject,int>.Broadcast(GameEvent.ENEMY_HIT, hit.gameObject,damage);
                //        Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE, hit.gameObject, damage);
                //        explosion = true;
                //    }

                //}

                
            }
        }
        if (explosion)
        {
            transform.gameObject.SetActive(false);
            Destroy(this);
            
        }

    }
}