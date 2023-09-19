using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    private Transform[] waypoints;
    private Transform currentTarget;
    private float playerNearRadius;
    private float waypointNearRadius;
    private float rotateSpeed = 10.0f;
    private float moveSpeed = 5.0f;

    public PatrolState(Transform[] waypoints,
        float playerNearRadius, float waypointNearRadius)
    {
        this.waypoints = waypoints;
        this.playerNearRadius = playerNearRadius;
        this.waypointNearRadius = waypointNearRadius;
        _stateId = FSMStateID.Patrol;
        SetTargetWaypoint();
    }

    public override void CheckTransitionRules(Transform player, Transform agent)
    {
        if(Vector3.Distance(agent.position, player.position) <= playerNearRadius)
        {
            EnemyTankController enemyTankController = agent.GetComponent<EnemyTankController>();
            enemyTankController.SetTransition(Transition.SawPlayer);
        }
    }

    public override void RunState(Transform player, Transform agent)
    {
        //Look for a new waypoint if near waypoint radius
        if (Vector3.Distance(agent.position, currentTarget.position)
            <= waypointNearRadius)
        {
            SetTargetWaypoint();
        }

        //Look at the target and move towards it
        Vector3 targetDirection = currentTarget.position - agent.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //Rotate the tank to face the targetDirection
        agent.rotation = Quaternion.Slerp(agent.rotation, targetRotation,
            Time.deltaTime * rotateSpeed);
        //Move the enemy forward
        agent.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    private void SetTargetWaypoint()
    {
        //Randomize a value from the array
        int randomIndex = Random.Range(0, waypoints.Length);
        //Make sure that the new target is not the same as the previous waypoint
        while (waypoints[randomIndex] == currentTarget)
        {
            randomIndex = Random.Range(0, waypoints.Length);
        }
        currentTarget = waypoints[randomIndex];
    }
}
