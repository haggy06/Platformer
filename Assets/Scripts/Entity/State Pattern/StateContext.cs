using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateContext
{
    private readonly Controller controller;
    public StateContext(Controller controller)
    {
        this.controller = controller;
    }

    private IState curState;
    public void Transition()
    {
        curState.StateStart(controller);
    }
    public void Transition(IState newState)
    {
        curState = newState;
        curState.StateStart(controller);
    }

    public IState GetCurState()
    {
        return curState;
    }
}
