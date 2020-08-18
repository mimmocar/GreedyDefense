using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public class MoveDestination : MonoBehaviour
//{

//    public Transform[] points;
//    private int destPoint = 0;
//    private NavMeshAgent agent;


//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();

//        // Disabling auto-braking allows for continuous movement
//        // between points (ie, the agent doesn't slow down as it
//        // approaches a destination point).
//        agent.autoBraking = false;

//        GotoNextPoint();
//    }


//    void GotoNextPoint()
//    {
//        // Returns if no points have been set up
//        if (points.Length == 0)
//            return;

//        // Set the agent to go to the currently selected destination.
//        agent.destination = points[destPoint].position;

//        // Choose the next point in the array as the destination,
//        // cycling to the start if necessary.
//        destPoint = (destPoint + 1);
//    }


//    void Update()
//    {
//        // Choose the next destination point when the agent gets
//        // close to the current one.
//        if (!agent.pathPending && agent.remainingDistance < 0.5f)
//            GotoNextPoint();
//    }
//}
public class MoveDestination : MonoBehaviour
{

    private Transform goal;
    private GameObject destination;
    private NavMeshAgent agent;
    private Animator animator;
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("food");
        goal = destination.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        animator = GetComponent<Animator>();
        animator.SetBool("destinationReached", false);
        agent.stoppingDistance = destination.transform.localScale.x / 2;
    }


    void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    //In attesa di trovare animazioni decenti
                    //animator.SetBool("destinationReached", true);
                }
            }
            else
            {
                //animator.SetBool("destinationReached", false);
            }
        }
    }

    
    //void OnCollisionEnter(Collision collision)
    //{
    //    StartCoroutine(CollisionHandling());
    //}

    //private IEnumerator CollisionHandling()
    //{
    //    agent.isStopped = true;
    //    yield return new WaitForSeconds(Random.Range(1, 5));
    //    agent.isStopped = false;
    //}

    //void OnCollisionExit(Collision collision)
    //{

    //    StopCoroutine(CollisionHandling());


    //}
}