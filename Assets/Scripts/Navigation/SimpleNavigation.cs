using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleNavigation : MonoBehaviour
{
    public NavMeshAgent thisAgent;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        thisAgent.SetDestination(thisAgent.destination);
        
    }
}
