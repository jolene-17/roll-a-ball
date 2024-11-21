using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachineController : MonoBehaviour
{
    private State _currentState;
    public GameObject enemy;
    public GameObject chaserEnemy;
    public Transform[] waypoints;
    public Transform player;
    public Material patrollingColour;
    public Material chasingColour;
    // Start is called before the first frame update
    void Start()
    {
        _currentState = new PatrolState(enemy, waypoints, patrollingColour);
        _currentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.OnUpdate();   
    }

    public void ChangeState(State newState)
    {
        _currentState.OnExit();

        _currentState = newState;
        _currentState.OnEnter();
    }

    public void ChangeToPatrol()
    {
        ChangeState(new PatrolState(enemy, waypoints, patrollingColour));
    }

    public void ChangeToChase()
    {
        ChangeState(new ChaseState(enemy, chaserEnemy, player, chasingColour));
    }

    public void ChangeToBig()
    {
        ChangeState(new BigState(enemy));
    }
}
