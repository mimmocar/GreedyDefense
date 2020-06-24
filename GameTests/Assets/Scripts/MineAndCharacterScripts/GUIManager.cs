using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{

    [SerializeField] VariableJoystick joystick;
    [SerializeField] GameObject character;
    private Vector2 touchPositionStart;
    private Vector2 touchPositionEnd;
    private bool display = false;
    private Rect mina, cubo, sfera;
    private bool evaluateSelection=false;
    private Vector3 worldPosition;

    void Update()
    {
        
        int count = Input.touchCount;
        if (count > 0) {
            Touch theTouch = Input.GetTouch(0);
            if(theTouch.phase == TouchPhase.Began) {

                touchPositionStart = theTouch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPositionStart);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.transform.tag == "floor")
                {
                    Debug.Log("Hit identified: " + hit);
                    worldPosition = hit.point;
                    // Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, hit.point);
                    // position.y = Screen.height - position.y;
                    Vector2 position = touchPositionStart;
                    position.y = Screen.height - position.y;
                    Debug.Log("Touch Position: " + position);
                    Debug.Log("Rect Position: " + (position.y - 50));
                    mina = new Rect(position.x - 300, position.y -200 , 200, 150);
                    cubo = new Rect(position.x - 100, position.y -200 , 200, 150);
                    sfera = new Rect(position.x + 100, position.y -200, 200, 150);
                    if (!joystick.isActive) display = true;
                }
                else
                {
                    display = false;
                }
            }
            else if (joystick.isActive)
            {
                display = false;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                touchPositionEnd = theTouch.position;
                touchPositionEnd.y = Screen.height - touchPositionEnd.y;
                evaluateSelection = true;
     
            }



        }
            
    }

    void OnGUI()
    {
        if (display)
        {
            GUI.Box(mina, "Mina");
            GUI.Box(cubo, "Cubo");
            GUI.Box(sfera, "Sfera");
        }
        if (evaluateSelection)
        {
            if (mina.Contains(touchPositionEnd))
            {
                Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 0);
                Debug.Log("Selected arma 1");
            }
            else if (cubo.Contains(touchPositionEnd))
            {
                Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 1);
                Debug.Log("Selected arma 2");
            }
            else if (sfera.Contains(touchPositionEnd))
            {
                Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 2);
                Debug.Log("Selected arma 3");
            }

            display = false;
            evaluateSelection = false;
        }
        

    }
}
