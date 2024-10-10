using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    
    protected StateMachine stateMachine;

    public virtual void EnterState()
    {

    }
    public virtual void FrameUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void ExitState() 
    {
        
    }
}
