using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //public void OnChangeScene()
    //{
    //    Messenger.Broadcast(GameEvent.CHANGE_SCENE);
    //}

    public void Select(string selectedButton)
    {
        SceneManager.LoadScene(selectedButton);
    }
}
