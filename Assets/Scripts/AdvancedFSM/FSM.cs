using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        FSMUpdate();
    }

}
