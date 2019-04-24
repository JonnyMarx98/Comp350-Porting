using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Code
{
    public class WaypointsC : Waypoints
    {

        [SerializeField]
        protected float _areaRadius = 40f;

        //list of waypoints that character can move to
        List<WaypointsC> _connections;

        public void Start()
        {
            //Collects all waypoint objects within the scene
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

            //Creates Empty List of patrol points
            _connections = new List<WaypointsC>();

            //Checks if the patrolpoint the object is moving to is a connected waypoint in the list
            for (int i = 0; i < waypoints.Length; i++)
            {

                WaypointsC nextPatrolpoint = waypoints[i].GetComponent<WaypointsC>();

                //when found a waypoint
                if (nextPatrolpoint != null)
                {
                    {
                        if (Vector3.Distance(this.transform.position, nextPatrolpoint.transform.position) <= _areaRadius && nextPatrolpoint != this);
                }
                    _connections.Add(nextPatrolpoint);
                }
            }

        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _areaRadius);
        }

        public WaypointsC NextPatrolpoint(WaypointsC previouswaypoint)
        {
            if (_connections.Count == 0)
            {
                //if there are no waypoints to connect to then return null and send error
                Debug.LogError("Where Da Waypoints at?");
                return null;
            }
            else if (_connections.Count == 1 && _connections.Contains(previouswaypoint))
            {
                //if thee is only one patrolpoint and it is the last one visited go back to it 
                return previouswaypoint;
            }
            else
            //look for a random patrol point that isn't the previous patrol point
            {
                WaypointsC nextPatrolpoint;
                int nextIn = 0;

                do
                {
                    nextIn = UnityEngine.Random.Range(0, _connections.Count);
                    nextPatrolpoint = _connections[nextIn];
                } while (nextPatrolpoint == previouswaypoint);

                return nextPatrolpoint;
            }
        }

    }
}