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

        var emission = attack.emission;
        emission.enabled = true;
    }

    void OnCollisionStay(Collision collision)
    {
        Messenger.Broadcast(GameEvent.HANDLE_FOOD_ATTACK);    
    }

    private void OnCollisionExit(Collision collision)
    {
        var emission = attack.emission;
        emission.enabled = false;
    }
}
