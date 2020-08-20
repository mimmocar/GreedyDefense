using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingButton : Button
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
        Messenger.AddListener(GameEvent.BERSERK_OFF, OnPointerUp);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BERSERK_OFF, OnPointerUp);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    public void OnPointerUp()
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(null); //il metodo serve solo a resettare lo stato iniziale del bottone
    }
}
