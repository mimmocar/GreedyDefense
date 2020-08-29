using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class JoystickCharacterState : MonoBehaviour
{

    [SerializeField] VariableJoystick joystick;  //gestire generalizzazione come visto a lezione
    [SerializeField] FloatingButton shootingButton;
    protected bool isMoving;
    protected bool isRotating;
    protected bool isGrounded;
    protected bool isBerserkOn;
    protected bool isShooting;

    protected float movement;
    protected float rotation;
    private int countDown;

    protected bool isStanding;
    private float gravity;
    private float minimumDistance;


    public float Gravity
    {
        get
        {
            return gravity;
        }
    }
    public bool IsStanding
    {
        get
        {
            return isStanding;
        }
    }

    public bool IsBerserkOn
    {
        get
        {
            return isBerserkOn;
        }
    }

    public bool IsShooting
    {
        get { return isShooting; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }

    }

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
    }

    public bool IsRotating
    {
        get
        {
            return isRotating;
        }
    }

    public float Movement
    {
        get
        {
            return movement;
        }
    }

    public float Rotation
    {
        get
        {
            return rotation;
        }
    }

    void Awake()
    {
        string filePath = "File/playerFeatures";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "gravity":
                    gravity = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "minimumDistance":
                    minimumDistance = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:   //aggiungere alla lettura  e handOffset, scaleWeapon e rotation params, bulletPoolSize  
                    break;

            }
        }
    }

    void Start()
    { 
        Messenger.AddListener(GameEvent.BERSERK_ON, OnBerserkOn);
        Messenger.AddListener(GameEvent.BERSERK_OFF, OnBerserkOff);
        

    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BERSERK_ON, OnBerserkOn);
        Messenger.RemoveListener(GameEvent.BERSERK_OFF, OnBerserkOff);
        
    }

    // Update is called once per frame
    void Update()
    {
        CollectInputs();
        CheckGrounded();
        Debug.Log(IsGrounded);
    }

    protected void CheckGrounded()
    {

        RaycastHit hit;
        //if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down), out hit, 0.1f) && hit.collider.tag == "floor")
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, minimumDistance) && hit.collider.tag == "floor")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    protected void CollectInputs()
    {

        movement = joystick.Vertical;
        rotation = joystick.Horizontal;
        isMoving = movement != 0;
        isRotating = rotation != 0;
        isShooting = shootingButton.IsShooting;
        if (isMoving == false && isRotating == false && joystick.isPressed)
            isStanding = true;
        else isStanding = false;

    }

    private void OnBerserkOn()
    {
        isBerserkOn = true;
        GetComponent<AutoShooting>().enabled = false;
        isShooting = false;
        
    }

    private void OnBerserkOff()
    {
        isBerserkOn = false;
        GetComponent<AutoShooting>().enabled = true; ;
    }

    
}
