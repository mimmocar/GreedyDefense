using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    
    private ParticleSystem attack;
    private Collider coll;
    private void Start()
    {

        attack = GetComponent<ParticleSystem>();
        var emission = attack.emission;
        emission.enabled = false;
        coll = GetComponent<Collider>();
        
    }


    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            var emission = attack.emission;
            emission.enabled = true;
            
        }

    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Dictionary<string, float> multDic = enemy.DamagesMultiplierDic;
            Messenger<float>.Broadcast(GameEvent.HANDLE_FOOD_ATTACK,multDic["foodAttackMultiplier"]);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("COLLISION EXIT");
            var emission = attack.emission;
            emission.enabled = false;
            
        }
    }
}
