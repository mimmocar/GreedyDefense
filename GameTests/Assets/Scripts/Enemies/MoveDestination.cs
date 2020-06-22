using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviour
{

    private Transform goal;
    private GameObject destination;
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("food");
        goal = destination.transform;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
}
