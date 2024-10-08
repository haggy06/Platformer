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

    private State curState;
    public void Transition()
    {
        curState.Act(controller);
    }
    public void Transition(State newState)
    {
        curState = newState;
        curState.Act(controller);
    }

    public State GetCurState()
    {
        return curState;
    }
}
