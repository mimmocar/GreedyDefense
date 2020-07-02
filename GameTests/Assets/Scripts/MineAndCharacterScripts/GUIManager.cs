using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    private bool paused;

    [SerializeField] VariableJoystick joystick;
    [SerializeField] GameObject character;
    [SerializeField] private Texture2D mineTexture, wallTexture, torretTexture;
    [SerializeField] private Text killsText;
    private Vector2 touchPositionStart;
    private Vector2 position;
    private Vector2 touchPositionEnd;
    private bool display = false;
    private Rect mine, wall, torret;
    private bool evaluateSelection = false;
    private Vector3 worldPosition;
    private bool shooting = false;

    void Start()
    {
        Messenger.AddListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.AddListener(GameEvent.STOP_SHOOTING, OnShootingStop);
        Messenger<GameObject, int>.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger<int, int>.AddListener(GameEvent.ENEMY_DIED, OnEnemyDied);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SHOOTING, OnShootingStart);
        Messenger.RemoveListener(GameEvent.STOP_SHOOTING, OnShootingStop);
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

    private void OnEnemyHit(GameObject enemy, int damage)
    {
        float health = enemy.GetComponent<Enemy>().Health;
        Image healthBar = enemy.GetComponent<Enemy>().healthBar;
        float startH = enemy.GetComponent<Enemy>().startHealth;
        healthBar.fillAmount = health / startH;
    }
    private void OnEnemyDied(int kills, int berserk)
    {
        if (killsText != null)
        {
            killsText.text = kills.ToString() + "/" + berserk.ToString();
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
        if (!paused) { 
       
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

}
