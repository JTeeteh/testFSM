using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFSM : FSM
{
    private FSMState currentState;
    public FSMState CurrentState
    {
        get { return currentState; }
    }
    private FSMStateID currentStateID;

    private List<FSMState> fsmStates;

    public AdvancedFSM()
    {
        fsmStates = new List<FSMState>();
    }

    public void AddFSMState(FSMState state)
    {
        //Check if null
        if(state == null) 
        {
            return;
        }

        //Check if the state is the first to be added in the list 
        //To setup the default state when the play mode begins
        if(fsmStates.Count == 0 ) 
        { 
            fsmStates.Add(state);
            currentState = state;
            currentStateID = state.StateId;
            return;
        }

        //Prevent adding the same state to the list
        if(fsmStates.Contains(state))
        {
            return;
        }
        fsmStates.Add(state);
    }

    public void RemoveState(FSMState state)
    {
        if(state == null)
        {
            return;
        }
        if (fsmStates.Contains(state))
        {
            fsmStates.Remove(state);
        }
    }

    //This method will try to chang the state the FSM is in based on the current state and transition
    public void PerformTransition(Transition transition)
    {
        //Check for null
        if(transition == Transition.None)
        {
            Debug.Log("Cannot transition to null");
            return;
        }

        //Check if the currentState has the transition
        FSMStateID id = currentState.GetOutputState(transition);

        //Check if null again
        if(id == FSMStateID.None)
        {
            Debug.Log("Current state does not support target state for this transition");
            return;
        }
        
        //Update the currentStateID and currentState
        currentStateID = id;
        foreach(FSMState state in fsmStates) 
        {
            if (state.StateId == currentStateID)
            {
                currentState = state;
                break;
            }
        }
    }
 
}
