using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    public GameObject enemy;
    public GameObject chaserEnemy;
    public Material chasingColour;
    public Transform player;
    private NavMeshAgent navMeshAgent;

    public ChaseState(GameObject enemy, GameObject chaserEnemy, Transform player, Material chasingColour)
    {
        this.enemy = enemy;
        this.chaserEnemy = chaserEnemy;
        this.player = player;
        this.chasingColour = chasingColour;
    }
    public override void OnEnter()
    {
        Debug.Log("Entered patrol state");
        //changes to red
        enemy.GetComponent<Renderer>().material = chasingColour;
        navMeshAgent = chaserEnemy.GetComponent<NavMeshAgent>();
    }

    public override void OnUpdate()
    {
        if(player!=null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public override void OnExit()
    {
        Debug.Log("Exited chase state");
    }
}
