using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    void OnCollisionStay(Collision collision)
    {
        Messenger.Broadcast(GameEvent.HANDLE_FOOD_ATTACK);    
    }
}
