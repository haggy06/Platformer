using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public virtual void EnterState()
    {

    }
    public virtual void ExitState()
    {

    }

    public virtual void OnUpdate()
    {

    }
    public virtual void OnFixedUpdate()
    {

    }

    protected virtual void TaskFinish()
    {
        if (TaskFinished != null)
        {
            TaskFinished.Invoke();
            TaskFinished = null;
        }
    }
    public event System.Action TaskFinished;
}

[System.Serializable]
public class Task
{        
    protected StateMachine stateMachine;

    public virtual void EnterState()
    {

    }
    public virtual void ExitState() 
    {
        
    }
}