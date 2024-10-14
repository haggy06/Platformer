using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [field : SerializeField]
    public bool IsActive { get; private set; }

    public virtual void EnterState(StateMachine stateMachine)
    {
        IsActive = true;
    }
    public virtual void ExitState(StateMachine stateMachine)
    {
        IsActive = false;
    }

    public virtual void OnFixedUpdate(StateMachine stateMachine)
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