using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    protected State initialState;

    private StateContext stateContext;
    public StateContext StateContext => stateContext;
    protected virtual void Awake()
    {
        stateContext = new StateContext(this);
        if (initialState != null)
            stateContext.Transition(initialState);
    }
}
