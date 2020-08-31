using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    private bool paused, over, won;
    [SerializeField] GameObject character;    

    private ObjectManager om;
    private GameControl gameControl;
    [SerializeField] private Texture2D mineTexture, missileTurrTexture, torretTexture;
    private Vector2 touchPositionStart;
    private Vector2 position;
    private Vector2 touchPositionEnd;
    private bool display = false;
    private Rect mine, missileTur, torret;
    int mineCost, missileTurCost, torretCost;
    private bool evaluateSelection = false;
    private Vector3 worldPosition;
    private bool shooting = false;

    private float rectPositionOffsetX;
    private float rectPositionOffsetY;
    private float rectWidth;
    private float rectHeight;
    private float rectColorShade;
    private int rectFontSize;
    private float rectColorTextShade;

    private JoystickCharacterState playerStatus;
    public int TEMP
    {
        get { return 1; }
        
    }

    void Start()
    {
        om = ObjectManager.Instance();
        gameControl = GameControl.Instance();
        mineCost = om.GetCost(0);
        missileTurCost = om.GetCost(1);
        torretCost = om.GetCost(2);
        Messenger.AddListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.AddListener(GameEvent.STOP_SHOOTING, OnShootingStop);


        string filePath = "File/guiManagerFeatures";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "rectPositionOffsetX":
                    rectPositionOffsetX = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rectPositionOffsetY":
                    rectPositionOffsetY = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rectWidth":
                    rectWidth = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rectHeight":
                    rectHeight = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rectColorShade":
                    rectColorShade = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rectColorTextShade":
                    rectColorTextShade = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rectFontSize":
                    rectFontSize = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }
        }

        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<JoystickCharacterState>();
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.RemoveListener(GameEvent.STOP_SHOOTING, OnShootingStop);

    }


    void LateUpdate()
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
                    position = touchPositionStart;
                    position.y = Screen.height - position.y;
                    Debug.Log("Touch Position: " + position);
                    
                    if (!playerStatus.IsMoving && !playerStatus.IsRotating && !playerStatus.IsShooting && !playerStatus.IsStanding)
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
            else if (playerStatus.IsMoving || playerStatus.IsRotating || playerStatus.IsShooting || playerStatus.IsStanding)  //controllare che l'accesso allo stato sia consentito
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
        

        paused = gameControl.IsGamePaused();
        over = gameControl.IsGameOver();
        won = gameControl.HasPlayerWon();

        if (!paused && !over && !won) { 
       
            if (display)
            {

                GUIStyle textStyle = new GUIStyle();
                //textStyle.fontSize = 50;
                textStyle.fontSize = rectFontSize;
                textStyle.alignment = TextAnchor.LowerRight;
                textStyle.normal.textColor = Color.white;
                //textStyle.hover.textColor = Color.black;
                textStyle.hover.textColor = Color.yellow;
                Debug.Log("GUI FETURE- RECT POS X" + rectPositionOffsetX);
                Debug.Log("GUI FETURE- RECT POS Y" + rectPositionOffsetY);
                Debug.Log("GUI FETURE- RECT POS X+ WIDTH" + (rectPositionOffsetX + rectWidth));
                Debug.Log("GUI FETURE- WIDTH" + rectWidth);
                Debug.Log("GUI FETURE- HEIGHT" + rectHeight);


                float positionY = position.y - rectPositionOffsetY;

                if (positionY < 0)
                    positionY = 0;


                //mine = new Rect(position.x - 300, position.y - 200, 200, 150);
                mine = new Rect(position.x - (rectPositionOffsetX + rectWidth), positionY, rectWidth, rectHeight);
                //missileTur = new Rect(position.x - 100, position.y - 200, 200, 150);
                missileTur = new Rect(position.x - rectPositionOffsetX, positionY, rectWidth, rectHeight);
                //torret = new Rect(position.x + 100, position.y - 200, 200, 150);
                torret = new Rect(position.x + rectPositionOffsetX, positionY, rectWidth, rectHeight);

                //GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                GUI.backgroundColor = new Color(rectColorShade, rectColorShade, rectColorShade);
                GUI.Box(mine, mineTexture);
                GUI.Box(missileTur, missileTurrTexture);
                GUI.Box(torret, torretTexture);

                //GUI.backgroundColor = new Color(0, 0, 0, 0);
                GUI.backgroundColor = new Color(rectColorTextShade, rectColorTextShade, rectColorTextShade);


                GUI.Box(mine, mineCost.ToString(), textStyle);  //convertire prezzo in int
                GUI.Box(missileTur, missileTurCost.ToString(), textStyle);
                GUI.Box(torret, torretCost.ToString(), textStyle);

            }

            if (evaluateSelection)
            {
                if (mine.Contains(touchPositionEnd))
                {
                    om.OnSpawnObject(worldPosition, 0);
                    Debug.Log("Selected arma 1");
                }
                else if (missileTur.Contains(touchPositionEnd))
                {
                    om.OnSpawnObject(worldPosition, 1);
                    Debug.Log("Selected arma 2");
                }
                else if (torret.Contains(touchPositionEnd))
                {
                    om.OnSpawnObject(worldPosition, 2);
                    Debug.Log("Selected arma 3");
                }

                display = false;
                evaluateSelection = false;
            }
        }


    }

    public void OnPauseButton()
    {
        _GameState gameState = gameControl.GetGameState();
        if (gameState == _GameState.Over) return;


        if (gameState == _GameState.Pause)
        {
            gameControl.ResumeGame();
            
        }
        else
        {
            gameControl.PauseGame();
            
        }
    }

    
}
