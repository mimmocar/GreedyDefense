using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform goal;
    void Start()
    {
        goal = GameObject.FindWithTag("food").transform;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

}
