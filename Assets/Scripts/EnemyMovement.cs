using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private float minDetectDistance;
    [SerializeField]
    private float maxChaseDistance;
    [SerializeField]
    private float growProbability;
    private EnemyStateMachineController _stateMachine;
    private float stateTimer = 0f;
    private System.Random random = new System.Random();

    void Start()
    {
        _stateMachine = GetComponent<EnemyStateMachineController>();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        // If state duration has elapsed, decide next state
        if (stateTimer >= 10f)
        {
            CheckProbability();
            stateTimer = 0f;
        }

        if (Vector3.Distance(transform.position, player.position) < minDetectDistance)
        {
            _stateMachine.ChangeToChase();
        }

        if (Vector3.Distance(transform.position, player.position) > maxChaseDistance)
        {
            _stateMachine.ChangeToPatrol();
        }
    }

    void CheckProbability(){
        float roll = (float)random.NextDouble();
        if (roll < growProbability)
        {
            _stateMachine.ChangeToBig();
        }
    }
}
