using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Dead }

    [Header("State")]
    [SerializeField]
    private State currentState;

    [Header("Movement")]
    [SerializeField]
    private float rotationSpeed = 10.0f;
    [SerializeField]
    private float moveSpeed = 5.0f;

    [Header("Waypoints")]
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private Transform currentWaypoint;

    [Header("Player")]
    [SerializeField]
    private Transform player;

    private void Start()
    {
        SetTargetWaypoint();
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehaviour();
                break;
            case State.Chase:
                ChaseBehaviour();
                break;
            case State.Attack:
                AttackBehaviour();
                break;
            case State.Dead:
                DeadBehaviour();
                break;
        }
    }
    private void SetTargetWaypoint()
    {
        //Set a random waypoint from the array
        int randomIndex = Random.Range(0, waypoints.Length);
        //Make sure that the new target is not the same as the previous waypoint target
        while(waypoints[randomIndex] == currentWaypoint)
            randomIndex = Random.Range(0, waypoints.Length);
        //finalize the waypoint
        currentWaypoint = waypoints[randomIndex];
    }
    private void PatrolBehaviour()
    {
        #region Behaviour
        //Move around the map through waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint.position);

        if(distanceToWaypoint > 3.0f)
        {
            //Move towards the waypoint

            //Get the rotation that faces towards the target
            Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint.position - transform.position);
            //Rotate the tank to face the targetRotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //Since it's already facing the target, we just move the tank forward
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        else
        {
            //Set a new waypoint
            SetTargetWaypoint();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        #endregion

        #region Transitions
        if (distanceToPlayer <= 10.0f)
            currentState = State.Chase;
        #endregion
    }

    private void ChaseBehaviour()
    {
        #region Behaviour
        //The player is our target so we move towards the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Get the rotation that faces towards the target
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        //Rotate the tank to face the targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //Since it's already facing the target, we just move the tank forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        #endregion

        #region Transitions
        if (distanceToPlayer > 10.0f)
            currentState = State.Patrol;
        else if (distanceToPlayer <= 5.0f)
            currentState = State.Attack;
        #endregion
    }

    private void AttackBehaviour()
    {
        //Assume we are shooting the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        #region Transitions
        if (distanceToPlayer > 5.0f)
            currentState = State.Chase;
        #endregion
    }
    private void DeadBehaviour()
    {

    }
}
