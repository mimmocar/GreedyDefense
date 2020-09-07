using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cannon
{
    public class rayCasting : MonoBehaviour
    {

        public Material lineRendererMaterial;
        protected LineRenderer line;
        [SerializeField] protected GameObject gun;
        // Start is called before the first frame update
        void Start()
        {
            line = createLine();
        }

        // Update is called once per frame
        void Update()
        {



            RaycastHit hit;
            Vector3 startingPos = gun.transform.position + new Vector3(0, 1f, 0);
            if (Physics.Raycast(startingPos, gun.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {

                line.SetPosition(0, startingPos);
                line.SetPosition(1, hit.point);
                if (hit.collider.CompareTag("player"))
                {
                    Debug.Log("Player Hit!");
                    //Do somwthing when the player is hit
                }

            }


        }



        private LineRenderer createLine()
        {
            LineRenderer myLine;
            myLine = new GameObject("Line").AddComponent<LineRenderer>();
            myLine.material = lineRendererMaterial;
            myLine.positionCount = 2;
            myLine.startWidth = 0.1f;
            myLine.endWidth = 0.1f;
            myLine.startColor = Color.red;
            myLine.endColor = Color.red;
            myLine.useWorldSpace = true;
            return myLine;
        }
    }
}
