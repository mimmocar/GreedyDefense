using UnityEngine;


public class MoveCharacter : MonoBehaviour
{
    protected CharacterController _charController;
    protected CharacterStatus status;

    [SerializeField] protected float walkSpeed = 6.0f;
    [SerializeField] protected float runBoost = 2f;
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected float rotationSensitivity = 9.0f;
    [SerializeField] protected float jumpForce = 5.0f;
    public float angle;
    Quaternion targetRotation;
    Transform cam;
    

    protected float vertSpeed = 0;

    void Awake()
    {
        _charController = GetComponent<CharacterController>();
        status = GetComponent<CharacterStatus>();
        
    }

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        
        if (status.IsMoving)
        {
            
            float vertMovement = status.Movement * walkSpeed;
            if (status.IsRunning)
            {
                vertMovement *= runBoost;
            }
            movement.z = vertMovement * Time.deltaTime;
            
        }


        if (!status.IsGrounded)
        {

            vertSpeed += gravity * Time.deltaTime;

        }
        else
        {
            vertSpeed = 0;
            if (status.IsJumping)
            {
                vertSpeed += jumpForce;
            }
        }
        movement.y += vertSpeed * Time.deltaTime;
    
        movement = transform.TransformDirection(movement);
        
        _charController.Move(movement);

        if (status.IsRotating)
        {
            
            transform.Rotate(0, status.Rotation * rotationSensitivity, 0);
        }

    }
}

