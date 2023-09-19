using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : AdvancedFSM
{
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float playerNearRadius;
    [SerializeField]
    private float waypointNearRadius;
    [SerializeField]
    private Transform player;

    protected override void Initialize()
    {
        //Constructing the FSM
        PatrolState patrol = new PatrolState(waypoints, playerNearRadius, waypointNearRadius);
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chase);

        ChaseState chase = new ChaseState(playerNearRadius);
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrol);

        AddFSMState(patrol);
        AddFSMState(chase);
    }

    protected override void FSMUpdate()
    {
        CurrentState.CheckTransitionRules(player, this.transform);
        CurrentState.RunState(player, this.transform);
    }

    public void SetTransition(Transition transition)
    {
        PerformTransition(transition);  
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerNearRadius);
    }
}
