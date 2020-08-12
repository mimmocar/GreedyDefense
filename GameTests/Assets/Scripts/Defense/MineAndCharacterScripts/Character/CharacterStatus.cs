using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStatus : MonoBehaviour
{

    protected bool isRunning;
    protected bool isMoving;
    protected bool isRotating;
    protected bool isGrounded;
    protected bool isJumping;

    protected float movement;
    protected float rotation;
    Animator anim;



    public bool IsRunning
    {
        get { return isRunning; }

    }

    public bool IsMoving
    {
        get { return isMoving; }

    }

    public bool IsRotating
    {
        get { return isRotating; }

    }

    public bool IsGrounded
    {
        get { return isGrounded; }

    }

    public bool IsJumping
    {
        get { return isJumping; }

    }

    public float Movement
    {
        get { return movement; }
    }

    public float Rotation
    {
        get { return rotation; }
    }


    void Start()
    {
        isGrounded = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        CheckGrounded();
    }

    protected void CheckGrounded()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position,
            transform.TransformDirection(Vector3.down), out hit, 0.2f)
                && hit.collider.CompareTag("floor"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    public void OnWalking(InputAction.CallbackContext context)
    {
        movement= context.ReadValue<Vector2>().y;
        rotation= context.ReadValue<Vector2>().x;
        if (movement != 0)
        {
            isMoving = true;
            //anim.SetBool("isMoving", true);
            
        }
        else
        {
            isMoving = false;
            //anim.SetBool("isMoving", false);

        }
        if (rotation != 0)
        {
            isRotating = true;
            //anim.SetBool("isMoving", true);
        }
        else
        {
            isRotating = false;
            //anim.SetBool("isMoving", false);
        }
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        float run = context.ReadValue<float>();
        isRunning = run > 0;
    }

    public void OnJumping(InputAction.CallbackContext context)
    {
        float jump= context.ReadValue<float>();
        isJumping = isGrounded && jump > 0;
    }

}
