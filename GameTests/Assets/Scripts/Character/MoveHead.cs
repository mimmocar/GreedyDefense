using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{


    public class MoveHead : MonoBehaviour
    {

        [SerializeField] protected float headRotationSensitivity=1f;
        [SerializeField] protected float headRotationLimitX = 45.0f;
        [SerializeField] protected float headRotationLimitY = 50.0f;

        [SerializeField]  protected GameObject headRef;

        protected float rotationX = 0;
        protected float rotationY = 0;

        // Start is called before the first frame update
        void Awake()
        {
            //headRef = GameObject.Find("Head");
        }




        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();

            if (movement.y!=0)
            {
                //Debug.Log("is rotating " + rotationX);
                rotationX -= movement.y * headRotationSensitivity;
                rotationX = Mathf.Clamp(rotationX, -headRotationLimitX, headRotationLimitX);

                headRef.transform.localEulerAngles = new Vector3(rotationX, 0,0);
            }
        }

    }
}
