using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerBerserk : MonoBehaviour
{
    [SerializeField] GameObject shootingButton;
    private GUIManager guiManager;
    // Start is called before the first frame update
    void Awake()
    {
        Messenger.AddListener(GameEvent.BERSERK_ON, OnBerserkOn);
        Messenger.AddListener(GameEvent.BERSERK_OFF, OnBerserkOff);
        guiManager = GetComponent<GUIManager>();

    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BERSERK_ON, OnBerserkOn);
        Messenger.RemoveListener(GameEvent.BERSERK_OFF, OnBerserkOff);
    }

    private void OnBerserkOn()
    {
        Debug.Log("DISATTIVO GUI");
        guiManager.enabled = false;
        //shootingButton.SetActive(false);
    }

    private void OnBerserkOff()
    {
        Debug.Log("RIATTIVO GUI");
        //shootingButton.SetActive(true) ;
        guiManager.enabled = true;
    }
}
