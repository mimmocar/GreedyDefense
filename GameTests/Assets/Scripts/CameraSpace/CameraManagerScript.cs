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



        void Awake()
        {
            Messenger.AddListener(GameEvent.BERSERK_ON, OnChangeCamera);
            Messenger.AddListener(GameEvent.BERSERK_OFF, OnChangeCamera);
        }

        void OnDestroy()
        {
            Messenger.RemoveListener(GameEvent.BERSERK_ON, OnChangeCamera);
            Messenger.RemoveListener(GameEvent.BERSERK_OFF, OnChangeCamera);            
        }

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

            
            Vector3 pos = toFollow.transform.position + cameraOffset;
            followingCamera.transform.position = Vector3.Slerp(followingCamera.transform.position, pos, SmoothFactor);
 
        }

        public void OnChangeCamera()
        {
            Debug.Log("Camera Changed");
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
