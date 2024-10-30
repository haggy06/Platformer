using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [field : SerializeField]
    public bool IsActive { get; private set; }
    [SerializeField]
    private bool canFinishState = true;
    public bool CanFinishState
    {
        get => canFinishState;
        protected set
        {
            canFinishState = value;
            if (canFinishState) // 종료 가능해졌을 경우
            {
                StateFinish();
            }
        }
    }

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

    protected virtual void StateFinish()
    {
        if (TaskFinishEvent != null)
        {
            TaskFinishEvent.Invoke();
            TaskFinishEvent = null;
        }
    }
    public event System.Action TaskFinishEvent;
}

[System.Serializable]
public class Task
{
    [SerializeField]
    protected bool canStopWhileRunning = true;
    protected bool isRunning = false;

    public bool CanStopWhileRunning => canStopWhileRunning;
    public bool IsRunning => isRunning;

    protected SequenceState sequence;

    public virtual void TaskStart(SequenceState sequence)
    {
        isRunning = true;
        this.sequence = sequence;
    }
    public virtual void OnFixedUpdate()
    {
        
    }
    public virtual void TaskComplete()
    {
        isRunning = false;
        sequence.CurTaskFInished();
    }

    public void ForceQuit()
    {
        isRunning = false;
    }
}