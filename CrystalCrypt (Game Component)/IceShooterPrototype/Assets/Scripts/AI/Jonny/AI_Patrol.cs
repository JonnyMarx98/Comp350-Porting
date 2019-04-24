using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;
using UnityEngine.AI;

public class AI_Patrol : StateBehaviour
{



    // Use this for initialization
    void Start ()
    {

	}

    void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendEvent("Chase");
        }
	}
}
