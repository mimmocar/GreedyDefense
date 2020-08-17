using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJoystickMovement : MonoBehaviour
{
    protected CharacterController _charController;
    public float velocity = 5;
    public float runBoost = 5;
    public float turnSpeed = 1;
    public float gravity = -9.8f;
    protected float rotationSensitivity = 3.0f;
    protected float vertSpeed = 0.0f;
    Vector2 input;
    float angle;
    Vector3 movement;
    Animator anim;
    Quaternion targetRotation;
    Transform cam;
    Camera followingCamera;
    protected GameObject weapon;
    public GameObject weaponPrefab;

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
        status = GetComponent<JoystickCharacterState>();
        _charController = GetComponent<CharacterController>();

        GameObject rHand = GameObject.Find("rHand");
        weapon = Instantiate(weaponPrefab, rHand.transform.position, Quaternion.Euler(-90, 0, 180));
        weapon.transform.parent = rHand.transform;
        weapon.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

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
            if (!status.IsMoving && !status.IsRotating)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("rotation", false);
                return;

            }
            GetInput();
            CalculateDirection();
            Rotate();
            Move();


        }
        else
        {
            anim.enabled = false;
            AlternativeMovement();
        }
    }

    void AlternativeMovement()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        if (status.IsMoving)
        {

            float vertMovement = status.Movement * velocity;

            movement.z = vertMovement * Time.deltaTime;

        }

        movement = transform.TransformDirection(movement);

        _charController.Move(movement);

        if (status.IsRotating)
        {

            transform.Rotate(0, status.Rotation * rotationSensitivity, 0);
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
        //transform.position += transform.forward * velocity * Time.deltaTime;



        movement = transform.forward * velocity * Time.deltaTime;
        //movement.y += vertSpeed * Time.deltaTime;
        _charController.Move(movement);
        anim.SetBool("isMoving", true);

    }
}



