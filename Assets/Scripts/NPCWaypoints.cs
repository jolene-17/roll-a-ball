using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoints : MonoBehaviour
{
    public static NPCWaypoints Instance{get; set;}
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject NPCWaypointsCluster;
}
