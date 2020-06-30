using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnChangeScene()
    {
        Messenger.Broadcast(GameEvent.CHANGE_SCENE);
    }
}
