using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigState : State
{
    public GameObject enemy;
    private Vector3 scaleChange;
    public BigState(GameObject enemy)
    {
        this.enemy = enemy;
    }
    // Start is called before the first frame update
    public override void OnEnter()
    {
        scaleChange = new Vector3(1, 1, 1);
        Debug.Log("Growing bigger");
    }

    public override void OnUpdate()
    {
        enemy.transform.localScale += scaleChange;
    }

    public override void OnExit()
    {
        Debug.Log("exiting large state");
    }
}
