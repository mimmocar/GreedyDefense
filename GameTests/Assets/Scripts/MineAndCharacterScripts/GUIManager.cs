﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{

    [SerializeField] VariableJoystick joystick;
    [SerializeField] GameObject character;
    [SerializeField] private Texture2D mineTexture, wallTexture, torretTexture;
    private Vector2 touchPositionStart;
    private Vector2 position;
    private Vector2 touchPositionEnd;
    private bool display = false;
    private Rect mine, wall, torret;
    private bool evaluateSelection = false;
    private Vector3 worldPosition;
    private bool shooting = false;

    private void Start()
    {
        Messenger.AddListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.AddListener(GameEvent.STOP_SHOOTING, OnShootingStop);
    }


    void Update()
    {

        int count = Input.touchCount;
        if (count > 0)
        {
            Touch theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {

                touchPositionStart = theTouch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPositionStart);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.transform.tag == "floor")
                {
                    Debug.Log("Hit identified: " + hit);
                    worldPosition = hit.point;
                    
                    // Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, hit.point);
                    // position.y = Screen.height - position.y;
                    position = touchPositionStart;
                    position.y = Screen.height - position.y;
                    Debug.Log("Touch Position: " + position);
                    Debug.Log("Rect Position: " + (position.y - 50));
                    if (!joystick.isActive && !shooting)
                    {
                        display = true;
                    }
                    else
                    {
                        display = false;
                    }
                }
                else
                {
                    display = false;
                }
            }
            else if (joystick.isActive || shooting)
            {
                display = false;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                touchPositionEnd = theTouch.position;
                touchPositionEnd.y = Screen.height - touchPositionEnd.y;
                if(display) evaluateSelection = true;

            }



        }

    }

    private void OnShootingStart()
    {
        shooting = true;
    }

    private void OnShootingStop()
    {
        shooting = false;
    }

    void OnGUI()
    {
        
        if (display)
        {
            mine = new Rect(position.x - 300, position.y - 200, 200, 150);
            wall = new Rect(position.x - 100, position.y - 200, 200, 150);
            torret = new Rect(position.x + 100, position.y - 200, 200, 150);


            GUI.Box(mine, mineTexture);
            GUI.Box(wall, wallTexture);
            GUI.Box(torret, torretTexture);

            
        }

        if (evaluateSelection)
        {
            if (mine.Contains(touchPositionEnd))
            {
                Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 0);
                Debug.Log("Selected arma 1");
            }
            else if (wall.Contains(touchPositionEnd))
            {
                Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 1);
                Debug.Log("Selected arma 2");
            }
            else if (torret.Contains(touchPositionEnd))
            {
                Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 2);
                Debug.Log("Selected arma 3");
            }

            display = false;
            evaluateSelection = false;
        }


    }
}
