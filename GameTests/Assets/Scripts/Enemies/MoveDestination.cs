using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
        //if (!agent.pathPending && agent.remainingDistance < 0.5f) ; in attesa di trovare animazioni decenti
    }

    
    
}