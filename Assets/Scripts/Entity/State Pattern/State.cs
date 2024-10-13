using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [field : SerializeField]
    public bool isActive { get; private set; }

    public virtual void EnterState()
    {
        isActive = true;
    }
    public virtual void ExitState()
    {
        isActive = false;
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