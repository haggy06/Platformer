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
        taskIndex++; //    �ε��� �� ĭ ����

        if (taskIndex < taskArray.Length) // taskIndex�� ������ ���� �ʾ��� ��� 
        {
            StartCoroutine("TaskTerm");
        }
        else // taskIndex�� ������ ���� ��� 
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
        if (!currentTask.IsRunning || currentTask.CanStopWhileRunning) // ���� Task�� ���� ���� �ƴϰų� �߰��� ��Ұ� ������ ���
        {
            currentTask.TaskStart(this); // ���� �ε����� �ش��ϴ� Task ����
        }
    }
    public void TaskInvoke(int taskIndex)
    {
        if (taskIndex < taskArray.Length) // �ε����� ����� �ʾ��� ���
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