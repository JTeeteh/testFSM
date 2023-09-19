using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public enum States
{
    Patrol,
    Chase,
    Attack
}
public class SimpleEnemyFSM : MonoBehaviour
{
    [SerializeField]
    private States currentState;

    [SerializeField]
    private float moveSpeed = 5.0f, rotateSpeed = 10.0f;
    [SerializeField]
    private float chaseDistance = 5.0f, waypointDistance = 3.0f;
    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private Transform player;
    private Transform currentTarget;

    private void Start()
    {
        SetTargetWaypoint();
    }

    private void Update()
    {
        switch(currentState)
        {
            case States.Patrol:
                PatrolBehaviour();
                break;
            case States.Chase:
                ChaseBehaviour();
                break;
            case States.Attack:
                AttackBehaviour();
                break;
        }
    }

    private void AttackBehaviour()
    {
        
    }

    private void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    private void MoveToTarget()
    {
        Vector3 targetDirection = currentTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //Rotate the tank to face the targetDirection
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotateSpeed);
        //Move the enemy forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
    private void ChaseBehaviour()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        MoveToTarget();

        if(distanceToPlayer > chaseDistance)
        {
            SetTargetWaypoint();
            currentState = States.Patrol;
        }
    }

    private void PatrolBehaviour()
    {
        //Move around the map:
        float distanceToWaypoint = Vector3.Distance(transform.position, currentTarget.position);

        if(distanceToWaypoint > waypointDistance)
        {
            MoveToTarget();
        }
        else
        {
            //Set a new waypoint target
            SetTargetWaypoint();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer <= chaseDistance)
        {
            SetTarget(player);
            currentState = States.Chase;
        }

        //3. When waypoint is reached, find another target position
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
        SetTarget(waypoints[randomIndex]);
    }
}
