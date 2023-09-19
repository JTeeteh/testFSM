using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState
{
    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
    protected FSMStateID _stateId;
    //shortcut: FSMStateID StateId => _stateId;
    public FSMStateID StateId
    {
        get { return _stateId; }
    }

    #region Sample
    /*
    void Sample()
    {
        List<int> intList = new List<int>();
        Dictionary<string, int> myDictionary = new Dictionary<string, int>();
        intList.Add(0);//"zero"
        intList.Add(5);//"five"
        intList.Add(10);//"a"
        intList.Add(-10);//"b"
        //List access through index
        //intList[2] = 10
        myDictionary.Add("zero", 0);
        myDictionary.Add("five", 5);
        int value = myDictionary["zero"];//0
    }
    */
    #endregion

    public void AddTransition(Transition transition, FSMStateID id)
    {
        //Check if the arguments passed are valid
        if(transition == Transition.None ||
            id == FSMStateID.None)
        {
            return;
        }
        //Prevent double entries to the map
        if(map.ContainsKey(transition))
        {
            return;
        }
        map.Add(transition, id);
    }

    public void RemoveTransition(Transition transition) 
    { 
        //Check for null transition
        if(transition == Transition.None)
        {
            return;
        }
        if(map.ContainsKey(transition))
        {
            map.Remove(transition);
        }
    }

    /// <summary>
    /// This method returns the ne state the the FSM should be if it receives the transition
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public FSMStateID GetOutputState(Transition transition)
    {
        //Check for null transition
        if (transition == Transition.None)
        {
            Debug.Log("Cannot transition to none");
            return FSMStateID.None;
        }
        if (map.ContainsKey(transition))
        {
            Debug.Log("Transition found to " + map[transition]);
            return map[transition];
        }
        Debug.Log("Default to none");
        return FSMStateID.None;
    }

    /// <summary>
    /// Decides if the state should transition to another
    /// </summary>
    public abstract void CheckTransitionRules(Transform player, Transform agent);
    /// <summary>
    /// Controls the behavior
    /// </summary>
    public abstract void RunState(Transform player, Transform agent);
}
