using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSpace
{

    public class CameraManagerScript : MonoBehaviour
    {

        [SerializeField] Camera[] cameras;
        [SerializeField] int followingCamera = 0;
        [SerializeField] GameObject toFollow;
        
        protected Vector3 cameraOffset;
        float SmoothFactor = 0.5f;

        protected int currentCamera;

        



        // Start is called before the first frame update
        void Start()
        {
            
            currentCamera = 0;
            for(int x=1;x<cameras.Length;x++)
            {
                cameras[x].enabled = false;
            }

            cameraOffset = cameras[followingCamera].transform.position - toFollow.transform.position;

        }
        

        private void LateUpdate()
        {
            if (currentCamera == followingCamera)
            {

                FollowObject(cameras[currentCamera]);
            }
     
        }

        protected void FollowObject(Camera followingCamera)
        {

            ////////Vector3 toTarget = toFollow.transform.position-followingCamera.transform.position;
            
            //toTarget.y = followingCamera.transform.position.y;
            Vector3 pos = toFollow.transform.position + cameraOffset;
            //pos.x -= toTarget.x;
            //pos.z -= toTarget.z;
            //pos.y = toTarget.y;
            //followingCamera.transform.Translate(toTarget.x,toTarget.y,toTarget.z,Space.World);

            followingCamera.transform.position = Vector3.Slerp(followingCamera.transform.position, pos, SmoothFactor);

            // This constructs a rotation looking in the direction of our target,
            ///////Quaternion targetRotation = Quaternion.LookRotation(toTarget);

            // This blends the target rotation in gradually.
            // lower values are slower/softer.
            ///////float sharpness = 0.1f;
            ////////followingCamera.transform.rotation = Quaternion.Lerp(followingCamera.transform.rotation, targetRotation, sharpness);

            // This gives an "stretchy" damping where it moves fast when far
            // away and slows down as it gets closer. You can also use 
            // Quaternion.RotateTowards() to get a more consistent speed.

            //transform.rotation=Quaternion.RotateTowards(transform.rotation, targetRotation, sharpness);
        }

        public void OnChangeCamera(InputAction.CallbackContext context)
        {
            int oldCamera = currentCamera;
            currentCamera++;
            if (currentCamera > cameras.Length - 1)
            {
                currentCamera = 0;
            }
            cameras[currentCamera].enabled = true;
            cameras[oldCamera].enabled = false;
        }


    }
}
