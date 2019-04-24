using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
	public class Patrol : MonoBehaviour
	{

        //Whether the character waits at each node or not
		[SerializeField]
		bool waiting;

        //how long the character will wait at each node for
		[SerializeField]
		float totalWaitingTime = 2.0f;

        //how likely the character is to switch between any of the nodes
		[SerializeField]
		float probSwitch = 0.5f;

        //the list of points that the character can patrol
		WaypointsC _currentPatrolpoint;
		WaypointsC _previousPatrolpoint;


		NavMeshAgent navMeshAgent;
		int patrolPointsvisited;
		bool isTravelling;
		bool isWaiting;
		bool moveForward;
		float waitTimer;

		// Use this for initialization
		public void Start()

		{
            //gets navmesh
			navMeshAgent = this.GetComponent<NavMeshAgent>();

            
			if (navMeshAgent == null)
			{
				Debug.LogError("is nav mesh agent attached to " + gameObject.name);
			}
			else
			{
				if (_currentPatrolpoint == null)
				{
                    //Takes all the objects in the scene tagged with "PatrolPoint"
					GameObject[] patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
					if (patrolPoints.Length > 0)
					{
						while (_currentPatrolpoint == null)
						{
                            //Randomly selects one patrolpoint, checks it has a connected patrol point and then sets that as the current waypoint
							int rando = UnityEngine.Random.Range(0, patrolPoints.Length);
							WaypointsC beginningPatrolpoint = patrolPoints[rando].GetComponent<WaypointsC>();

							if (beginningPatrolpoint != null)
							{
								_currentPatrolpoint = beginningPatrolpoint;
							}
						}
					}

					else
					{
						Debug.LogError("Can't Find Waypoints");
					}
				}

				SetDestination();
			}
		}

		// Update is called once per frame
		void Update()
		{
            //check if close to where we are going
			if (navMeshAgent.enabled && isTravelling && navMeshAgent.remainingDistance < 1.0f)
			{
				isTravelling = false;
				patrolPointsvisited++;

				if (isWaiting)
				{
					waiting = true;
					waitTimer = 0f;
				}
				else
				{

					SetDestination();
				}
			}

			if (waiting)
			{
				waitTimer += Time.deltaTime;
				if (waitTimer >= totalWaitingTime)
				{
					waiting = false;


					SetDestination();
				}
			}
		}

		private void SetDestination()
		{
            //checks for next waypoint and gives the previous waypoint as the current waypoint  and then set current patrolpoint to be the new patrol point.
			if (patrolPointsvisited > 0)
			{
				WaypointsC nextPatrolpoint = _currentPatrolpoint.NextPatrolpoint(_previousPatrolpoint);
				_previousPatrolpoint = _currentPatrolpoint;
				_currentPatrolpoint = nextPatrolpoint;
			}

			Vector3 targetVector = _currentPatrolpoint.transform.position;
			if (navMeshAgent.enabled)
				navMeshAgent.SetDestination(targetVector);

			isTravelling = true;
		}
	}
}