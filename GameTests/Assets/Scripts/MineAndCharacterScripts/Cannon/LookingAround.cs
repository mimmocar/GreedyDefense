using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cannon
{
    public class LookingAround : MonoBehaviour
    {

        [SerializeField]  protected float min = -60;
        [SerializeField]  protected float max = 60;
        protected float increment = -1;
        protected float rotation;
        protected bool rotating;
        [SerializeField]  protected float rotatingSpeed = 20;
        // Start is called before the first frame update
        [SerializeField] GameObject head;
        void Start()
        {
            rotation = head.transform.localEulerAngles.y;
            min += rotation;
            max += rotation;
            rotating = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(rotating)
            {
                rotation += increment * rotatingSpeed*Time.deltaTime;
                if(rotation<min)
                {
                    increment = 1;
                }
                else if (rotation >max)
                {
                    increment = -1;
                }

                head.transform.rotation = Quaternion.Euler(0, rotation, 0);
   
            }
            
        }
    }
}
