using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Patrol, Chase, Attack }
public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private EnemyState currentState;
    [SerializeField]
    private float rotationSpeed = 10.0f;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform player;

    [SerializeField]
    private Transform target;
    public Transform Target {
        get => target;
        ////
        /// The "=>" is a shortcut for 
        /// get { return target; }
        ///
        private set => target = value;
        ////
        /// The "=>" is a shortcut for 
        /// set { target = value; }
        ///
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }


    private void Start()
    {
        SetTargetWaypoint();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                break;
            case EnemyState.Attack:
                break;
        }
    }

    private void MoveToTarget()
    {
        //Get the rotation that is facing towards the target
        Quaternion targetRotation = Quaternion.LookRotation(Target.position -
            transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
        //Make our tank move forward since it should already face the target
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    private void ChaseBehavior()
    {
        Target = player;
        MoveToTarget();
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        //Switch state
        if (distanceToTarget > 10.0f)
        {
            currentState = EnemyState.Patrol;
        }
        if(distanceToTarget < 5.0f)
        {
            currentState = EnemyState.Attack;
        }
    }

    private void SetTargetWaypoint()
    {
        //randomize the target position and move towards it
        int randomIndex = Random.Range(0, waypoints.Length);

        //Make sure that the new target is not the same as the previous target
        while(waypoints[randomIndex] == target)
            randomIndex = Random.Range(0, waypoints.Length);

        //set our target to the random waypoint
        Target = waypoints[randomIndex];
    }

    private void PatrolBehavior()
    {
        //behaviour
        //Keep track of our distance to the target
        float distanceToTarget = Vector3.Distance(transform.position,
            target.position);

        //If we are far from the target, move towards it
        if(distanceToTarget > 3.0f)
        {
            MoveToTarget();
        }
        else
        {
            //Set a new waypoint if we are near the target
            SetTargetWaypoint();
        }

        //transition
        //Get distance from player
        if(Vector3.Distance(transform.position, player.position) < 10.0f)
        {
            currentState = EnemyState.Chase;
        }
    }
}
