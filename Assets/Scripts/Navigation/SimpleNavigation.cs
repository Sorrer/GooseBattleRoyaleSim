﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleNavigation : MonoBehaviour
{
    public NavMeshAgent thisAgent;

    public bool directLook = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(thisAgent.destination != transform.position)
        {
            if (directLook)
            {
                FaceTarget(thisAgent.destination);
            }
            thisAgent.SetDestination(thisAgent.destination);
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 30 * Time.deltaTime);
    }
}
