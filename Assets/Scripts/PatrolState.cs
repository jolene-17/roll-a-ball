using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    public NavMeshAgent navMeshAgent;
    public GameObject enemy;
    public Transform[] waypoints;
    public Material patrollingColour;
    private Rigidbody _rb;
    private int _currentWaypointIndex;
    // private float _speed = 2f;
    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public PatrolState(GameObject enemy, Transform[] waypoints, Material patrollingColour)
    {
        this.enemy = enemy;
        this.waypoints = waypoints;
        this.patrollingColour = patrollingColour;
    }
    public override void OnEnter()
    {
        navMeshAgent = enemy.GetComponentInParent<NavMeshAgent>();

        _rb = enemy.GetComponent<Rigidbody>();
        //colour turns green
        enemy.GetComponent<Renderer>().material = patrollingColour;
        Debug.Log("Entered patrol state");
    }

    public override void OnUpdate()
    {
        //waiting logic
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter < _waitTime) return;
            _waiting = false;
        }
        //walks around waypoints
        // Get current waypoint and calculate movement
        Transform wp = waypoints[_currentWaypointIndex];
        Vector3 directionToWaypoint = (wp.position - enemy.transform.position).normalized;

        // Apply force for movement
        navMeshAgent.SetDestination(directionToWaypoint);
        if(enemy.transform.position == wp.position)
        {
            _waitCounter = 0f;
            _waiting = true;
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length; 
        }
        else if(Vector3.Distance(enemy.transform.position, wp.position) < 0.5f)
        {
            enemy.transform.position = wp.position;
        }

        

        // if (Vector3.Distance(enemy.transform.position, wp.position) > 0.5f) // Allow for a threshold to avoid jittering
        // {
        //     _rb.AddForce(directionToWaypoint * _speed, ForceMode.Acceleration);
        //     enemy.transform.LookAt(new Vector3(wp.position.x, enemy.transform.position.y, wp.position.z));
        // }
        // else
        // {
        //     enemy.transform.position = wp.position; // Snap to waypoint when close enough
        //     _waitCounter = 0f;
        //     _waiting = true;
        //     _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        // }
    }

    public override void OnExit()
    {
        Debug.Log("Exited patrol state");
    }
}


