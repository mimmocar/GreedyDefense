using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    private GameObject food;
    private ParticleSystem attack;
    
    private void Start()
    {
        food = GameObject.FindGameObjectWithTag("food");
        attack = food.GetComponent<ParticleSystem>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var emission = attack.emission;
            emission.enabled = false;
        }
    }
}
