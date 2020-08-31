using System.Globalization;
using UnityEngine;
using DamagePackage;

public class CharacterJoystickMovement : MonoBehaviour
{
    protected CharacterController _charController;
    private GameControl gameControl;
    private float velocity;
    private float runBoost;
    private float turnSpeed; //implementare lettura da file dei parametri di configurazione
    private float gravity;
    protected float rotationSensitivity;
    protected float vertSpeed = 0.0f;
    private _Damage berserkDamage = new _Damage(DamageType.Berserk, "Berserk Attack", 0f);
    Vector2 input;
    float angle;
    Vector3 movement;
    Animator anim;
    Quaternion targetRotation;
    Transform cam;
    Camera followingCamera;


    protected JoystickCharacterState status;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        followingCamera = Camera.main;
        cam = GameObject.Find("FollowingCamera").transform;
        gameControl = GameControl.Instance();
        status = GetComponent<JoystickCharacterState>();
        _charController = GetComponent<CharacterController>();
        gravity = status.Gravity;

        TextAsset data = Resources.Load<TextAsset>("File/characterFeatures");
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "velocity":
                    velocity = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "runBoost":
                    runBoost = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "turnSpeed":
                    turnSpeed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "rotationSensitivity":
                    rotationSensitivity = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("ROTATION: " + rotationSensitivity);
                    break;
                default:
                    //possibile implementare come per enemy un dizionario di boost possibili
                    break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        _GameState gS = gameControl.GetGameState();
        if (gS == _GameState.Over || gS == _GameState.Won) return;
        if (!status.IsGrounded)
        {

            vertSpeed += gravity * Time.deltaTime;
            Debug.Log("STIAMO VOLANDO");

        }
        else
        {
            vertSpeed = 0;

        }
        _charController.Move(transform.TransformDirection(new Vector3(0, vertSpeed * Time.deltaTime, 0)));
        if (followingCamera.enabled)
        {
            anim.enabled = true;

            if (!status.IsShooting)
            {
                if (!status.IsMoving && !status.IsRotating)
                {
                    anim.SetBool("isMoving", false);
                    anim.SetBool("rotation", false);
                    return;

                }
                else
                {
                    GetInput();
                    CalculateDirection();
                    Rotate();
                    Move();
                }
                //GetInput();
                //CalculateDirection();
                //Rotate();
                //Move();
            }
            else
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isRotating", false);
                anim.SetBool("isShooting", true);
            }

        }
        else
        {
                anim.SetBool("isShooting", false);
                AlternativeMovement();
            
        }
    }

    

    void AlternativeMovement()
    {
        anim.SetBool("isMoving", false);
        anim.SetBool("rotation", false);

        Vector3 movementD = new Vector3(0, 0, 0);
        if (status.IsMoving)
        {

            float vertMovement = status.Movement * (velocity + runBoost);

            movementD.z = vertMovement * Time.deltaTime;

        }

        movementD = transform.TransformDirection(movementD);

        _charController.Move(movementD);

        if (status.IsRotating)
        {

            transform.Rotate(0, status.Rotation * rotationSensitivity, 0);
        }

        Debug.Log("MOVIMENTO : " + movementD);

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (status.IsBerserkOn)
        {
            if(hit.gameObject.tag == "Enemy")
            {
                Messenger<GameObject, _Damage>.Broadcast(GameEvent.HANDLE_DAMAGE,hit.gameObject, berserkDamage); //la quantità non è rilevante in questo caso
            }
        }
        
    }

    void GetInput()
    {
        input.x = status.Rotation;
        input.y = status.Movement;
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
        //Debug.Log(angle);
        if (angle < 0) anim.SetBool("rotation", true);
        else anim.SetBool("rotation", false);

    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
    private void Move()
    {
        
        movement = transform.forward * velocity * Time.deltaTime;
        _charController.Move(movement);
        anim.SetBool("isMoving", true);

    }
}



