using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class JyEnemy : MonoBehaviour
{
    private int scaleMulti = 2;
    private Vector3 scale;
    [SerializeField]
    private float detectionRadius = 10f;
    private Transform player;
    private NavMeshAgent agent;
    [SerializeField]
    private Material patrollingColour;
    [SerializeField]
    private Material chasingColour;
    [SerializeField]
    private float patrolTimer = 10f;
    private float currentTimer = 0f;
    [SerializeField]
    private float growTimer = 0f;
    [SerializeField]
    private float timeUntilThisGuyGrowsBig = 10f;
    List<Transform> waypointsList = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        GameObject waypointsCluster = NPCWaypoints.Instance.GetComponent<NPCWaypoints>().NPCWaypointsCluster;

        foreach(Transform t in waypointsCluster.transform)
        {
            waypointsList.Add(t);
        }
        GetComponent<Renderer>().material = patrollingColour;
        Vector3 firstPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(firstPosition);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceFromPlayer < detectionRadius) //chase state
        {
            Debug.Log("jy Enter chasing state");
            GetComponent<Renderer>().material = chasingColour;
            if (player!=null)
            {
                agent.SetDestination(player.transform.position);
            }
        }
        else //patrol state
        {
            currentTimer += Time.deltaTime;
            if(currentTimer >= patrolTimer)
            {
                Debug.Log(currentTimer + "Patrol timer");
                Debug.Log("jy Enter patrolling state");
                GetComponent<Renderer>().material = patrollingColour;
                agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
                currentTimer = 0f;
            }
        }
        growBigger();        
    }

    void growBigger()
    {
        growTimer += Time.deltaTime;
        growTimer %= timeUntilThisGuyGrowsBig+0.01f;

        if(growTimer >= timeUntilThisGuyGrowsBig)
        {
            int chanceThisGuyGrowsBig = Random.Range(0,9);
            if(chanceThisGuyGrowsBig <= 2 )
            {   
                agent.speed -= 0.5f;
                scale = new Vector3(0.5f*scaleMulti,1*scaleMulti,0.5f*scaleMulti);
                transform.localScale = scale;
                scaleMulti *= 2;
                Debug.Log("Fella grew by" + scaleMulti);
            }
        }
    }
}
