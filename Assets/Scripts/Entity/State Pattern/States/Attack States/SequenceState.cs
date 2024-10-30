using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceState : State
{
    [SerializeField]
    private Task[] taskArray;
    [SerializeField]
    private float defaultTerm = 0.5f;
    [SerializeField]
    private bool autoInvoke = true;

    private int taskIndex = 0;
    protected Task currentTask;

    public void CurTaskFInished()
    {
        taskIndex++; //    인덱스 한 칸 증가

        if (taskIndex < taskArray.Length) // taskIndex가 끝까지 가지 않았을 경우 
        {
            StartCoroutine("TaskTerm");
        }
        else // taskIndex가 끝까지 갔을 경우 
        {
            StateFinish();
        }
    }
    private IEnumerator TaskTerm()
    {
        yield return YieldReturn.WaitForSeconds(defaultTerm);

        if (IsActive)
            TaskInvoke();
    }

    public void TaskInvoke()
    {
        currentTask = taskArray[taskIndex];
        if (!currentTask.IsRunning || currentTask.CanStopWhileRunning) // 현재 Task가 실행 중이 아니거나 중간에 취소가 가능할 경우
        {
            currentTask.TaskStart(this); // 현재 인덱스에 해당하는 Task 실행
        }
    }
    public void TaskInvoke(int taskIndex)
    {
        if (taskIndex < taskArray.Length) // 인덱스를 벗어나지 않았을 경우
        {
            this.taskIndex = taskIndex;
            TaskInvoke();
        }
    }

    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);

        taskIndex = 0;
        TaskInvoke();
    }
    public override void ExitState(StateMachine stateMachine)
    {
        base.ExitState(stateMachine);
        if (currentTask != null && currentTask.IsRunning)
        {
            currentTask.ForceQuit();
        }
    }

    public override void OnFixedUpdate(StateMachine stateMachine)
    {
        base.OnFixedUpdate(stateMachine);

        currentTask?.OnFixedUpdate();
    }
}