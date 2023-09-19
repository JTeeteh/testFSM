using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private float rotateSpeed = 10.0f;
    private float moveSpeed = 5.0f;
    private float playerNearRadius;

    public ChaseState(float playerNearRadius)
    {
        this.playerNearRadius = playerNearRadius;
        _stateId = FSMStateID.Chase;
    }

    public override void CheckTransitionRules(Transform player, Transform agent)
    {
        float distance = Vector3.Distance(agent.position, player.position);
        if (distance >= playerNearRadius)
        {
            agent.GetComponent<EnemyTankController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void RunState(Transform player, Transform agent)
    {
        //Look at the target and move towards it
        Vector3 targetDirection = player.position - agent.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //Rotate the tank to face the targetDirection
        agent.rotation = Quaternion.Slerp(agent.rotation, targetRotation,
            Time.deltaTime * rotateSpeed);
        //Move the enemy forward
        agent.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
}
