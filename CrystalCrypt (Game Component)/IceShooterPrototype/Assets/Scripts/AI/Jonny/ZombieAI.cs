using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieAI : MonoBehaviour
{
	private float speed;
    private float maxSpeed;
    NavMeshAgent agent;
    GameObject[] Players;
    bool Attacked;
    public float AttackTime = 1f;
    private float InitAttackTime;
    ZombieManager zombieManager;
	int rand;

    // Use this for initialization
    void Start()
	{
        zombieManager = GameObject.Find("EnemyManager").GetComponent<ZombieManager>();
		agent = GetComponent<NavMeshAgent>();
        maxSpeed = 10f;
		speed = zombieManager.enemySpeed;
		if (speed > maxSpeed) { speed = maxSpeed; }
        agent.speed = speed;       
        InitAttackTime = AttackTime;
        Attacked = false;
		rand = Random.Range(1, 4);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !Attacked)
        {
            Attacked = true;
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            playerHealth.TakeDamage(15.0f, Vector3.zero, -1);
            
        }
    }

    GameObject ClosestPlayer()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        for (int i = 0; i < Players.Length; i++)
        {
            // Find the distance to this player
            Vector3 diff = Players[i].transform.position - transform.position;
            float playerDistance = diff.sqrMagnitude;

            // if the distance to this player is less than the previous player checked set this player as the closest
            if (playerDistance < distance)
            {
                closest = Players[i];
                distance = playerDistance;
            }
        }
        return closest;
    }

	float TargetPlayerDistance()
	{
		Vector3 diff = ClosestPlayer().transform.position - transform.position;
		float playerDistance = diff.sqrMagnitude;
		return playerDistance;
	}

	// work in progress currently broken
	void Front()
	{
		float distance = 5f;
		Vector3 distanceInFront = ClosestPlayer().transform.forward * distance;
		agent.SetDestination (ClosestPlayer ().transform.position + distanceInFront);
	}
	void Behind()
	{
		float distance = -25f;
		Vector3 distanceBehind = ClosestPlayer().transform.forward * distance;
		agent.SetDestination (ClosestPlayer ().transform.position + distanceBehind);
	}


    // Update is called once per frame
    void Update()
	{
        Players = GameObject.FindGameObjectsWithTag("Player");
        agent.SetDestination(ClosestPlayer().transform.position);
		//agent.SetDestination (ClosestPlayer ().transform.position);
		print (TargetPlayerDistance ());

		/*
		if (TargetPlayerDistance () > 100f && rand <= 2) 
		{
			Front ();
			//print ("front");
		} 
		if (TargetPlayerDistance () > 100f && rand >= 3)
		{
			Behind ();
			//print ("behind");
		}
		*/

		//Front();
        //print(speed);
        // Attacking delay
        if (Attacked)
        {
            AttackTime -= Time.deltaTime;
        }
        if (AttackTime <= 0.0f)
        {
            Attacked = false;
            AttackTime = InitAttackTime;
        }
        //print("Attacked = " + Attacked);
    }

    public void OnDestroy()
    {
        //zombieManager.EnemyCount--;
    }
}
