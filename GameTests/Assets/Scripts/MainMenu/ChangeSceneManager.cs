using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Messenger.AddListener(GameEvent.CHANGE_SCENE, ChangeScene);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
