
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour {

    public bool IsUsed;

	// Use this for initialization
	void Awake ()
    {
        IsUsed = false;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsUsed = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
