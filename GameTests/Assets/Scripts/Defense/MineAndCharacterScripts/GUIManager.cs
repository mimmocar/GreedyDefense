using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    private bool paused, over, won;
    [SerializeField] GameObject joystickGO;
    [SerializeField] GameObject buttonGO;
    [SerializeField] VariableJoystick joystick;
    [SerializeField] FloatingButton shootingButton;
    [SerializeField] GameObject character;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject levelWonUI;

    private ObjectManager om;
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

    private JoystickCharacterState playerStatus;
    public int TEMP
    {
        get { return 1; }
        
    }

    void Start()
    {
        om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //implementare singleton
        mineCost = om.GetCost(0);
        missileTurCost = om.GetCost(1);
        torretCost = om.GetCost(2);
        Messenger.AddListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.AddListener(GameEvent.STOP_SHOOTING, OnShootingStop);

        Messenger.AddListener(GameEvent.GAME_OVER, OnHandleGameOver);
        Messenger<int>.AddListener(GameEvent.LEVEL_WON, OnHandleLevelWon);

        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<JoystickCharacterState>();
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.RemoveListener(GameEvent.STOP_SHOOTING, OnShootingStop);

        Messenger.RemoveListener(GameEvent.GAME_OVER, OnHandleGameOver);
        Messenger<int>.RemoveListener(GameEvent.LEVEL_WON, OnHandleLevelWon);
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
                    position = touchPositionStart;
                    position.y = Screen.height - position.y;
                    Debug.Log("Touch Position: " + position);
                    Debug.Log("Rect Position: " + (position.y - 50));
                    if (!playerStatus.IsMoving && !playerStatus.IsRotating && !playerStatus.IsShooting)
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
            else if (playerStatus.IsMoving || playerStatus.IsRotating || playerStatus.IsShooting)  //controllare che l'accesso allo stato sia consentito
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
        

        paused = GameControl.IsGamePaused();
        over = GameControl.IsGameOver();
        won = GameControl.HasPlayerWon();

        if (!paused && !over && !won) { 
       
            if (display)
            {

                GUIStyle textStyle = new GUIStyle();
                textStyle.fontSize = 50;
                textStyle.alignment = TextAnchor.LowerRight;
                textStyle.normal.textColor = Color.black;
                textStyle.hover.textColor = Color.black;

                mine = new Rect(position.x - 300, position.y - 200, 200, 150);
                missileTur = new Rect(position.x - 100, position.y - 200, 200, 150);
                torret = new Rect(position.x + 100, position.y - 200, 200, 150);

                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                GUI.Box(mine, mineTexture);
                GUI.Box(missileTur, missileTurrTexture);
                GUI.Box(torret, torretTexture);

                GUI.backgroundColor = new Color(0, 0, 0, 0);
                
                GUI.Box(mine, mineCost.ToString(), textStyle);  //convertire prezzo in int
                GUI.Box(missileTur, missileTurCost.ToString(), textStyle);
                GUI.Box(torret, torretCost.ToString(), textStyle);

            }

            if (evaluateSelection)
            {
                if (mine.Contains(touchPositionEnd))
                {
                    //Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 0);
                    om.OnSpawnObject(worldPosition, 0);
                    Debug.Log("Selected arma 1");
                }
                else if (missileTur.Contains(touchPositionEnd))
                {
                    //Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 1);
                    om.OnSpawnObject(worldPosition, 1);
                    Debug.Log("Selected arma 2");
                }
                else if (torret.Contains(touchPositionEnd))
                {
                    //Messenger<Vector3, int>.Broadcast(GameEvent.SPAWN_REQUESTED, worldPosition, 2);
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
        _GameState gameState = GameControl.GetGameState();
        if (gameState == _GameState.Over) return;


        if (gameState == _GameState.Pause)
        {
            GameControl.ResumeGame();
            PauseMenu.Hide();
        }
        else
        {
            GameControl.PauseGame();
            PauseMenu.Show();
        }
    }

    public void OnHandleGameOver()
    {
        _GameState gameState = GameControl.GetGameState();
        if (gameState == _GameState.Over)
        {
            gameOverUI.SetActive(true);
            joystickGO.SetActive(false);
            buttonGO.SetActive(false);
        }
        else
        {
            gameOverUI.SetActive(false);
            joystickGO.SetActive(true);
            buttonGO.SetActive(true);
            return;
        }
    }

    public void OnHandleLevelWon(int score)
    {

        _GameState gameState = GameControl.GetGameState();

        if (gameState == _GameState.Won)
        {
            levelWonUI.SetActive(true);
            joystickGO.SetActive(false);
            buttonGO.SetActive(false);
            for (int i = 0; i < 3; i++)
            {
                if (i + 1 <= score)
                {
                    Transform border = levelWonUI.transform.GetChild(i + 4);
                    Transform starImage = border.GetChild(0);
                    starImage.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            levelWonUI.SetActive(false);
            joystickGO.SetActive(true);
            buttonGO.SetActive(true);
            return;
        }

       
    }

}
