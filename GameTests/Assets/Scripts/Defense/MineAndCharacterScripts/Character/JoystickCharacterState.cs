using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickCharacterState : MonoBehaviour
{

    [SerializeField] VariableJoystick joystick;  //gestire generalizzazione come visto a lezione
    [SerializeField] FloatingButton shootingButton;
    protected bool isMoving;
    protected bool isRotating;
    protected bool isGrounded;
    protected bool isBerserkOn;
    // Temporary added for Automatic Shooting character
    protected bool isShooting;

    protected float movement;
    protected float rotation;
    private int countDown;

    public bool IsBerserkOn
    {
        get
        {
            return isBerserkOn;
        }
    }

    // Temporary added for Automatic Shooting character
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
    // Start is called before the first frame update
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
        if (Physics.Raycast(transform.position,
            transform.TransformDirection(Vector3.down), out hit, 0.1f)
                && hit.collider.tag == "floor")
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
