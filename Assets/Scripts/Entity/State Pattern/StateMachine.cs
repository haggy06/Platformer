using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    protected StateType currentStateType = StateType.Patrol;
    protected State currentState;

    [SerializeField]
    protected State patrolState;
    [SerializeField]
    protected State chaseState;
    [SerializeField]
    protected State attackState;

    [Space(10)]
    [SerializeField]
    protected TAG targetTag = TAG.Player;
    [field: SerializeField]
    public Transform Target { get; protected set; }

    protected virtual void Awake()
    {
        ResetState(currentStateType);
    }
    protected virtual void FixedUpdate()
    {
        currentState.OnFixedUpdate(this);
    }

    public void ResetState(StateType newStateType)
    {
        currentStateType = newStateType;
        switch (currentStateType)
        {
            case StateType.Patrol:
                currentState = patrolState;
                break;
            case StateType.Chase:
                currentState = chaseState;
                break;
            case StateType.Attack:
                currentState = attackState;
                break;
        }

        currentState.EnterState(this);
    }
    public void ChangeState(StateType newStateType)
    {
        if (!currentState.CanFinishState) // ���� ���¸� ������ �� ���� ���
        {
            currentState.TaskFinishEvent += () => ChangeState(newStateType); // ���� ���°� ���� �������� �� �� �޼ҵ带 ��ȣ���Ѵ�.
            return;
        }

        currentState.ExitState(this);

        currentStateType = newStateType;
        switch (currentStateType)
        {
            case StateType.Patrol:
                currentState = patrolState;
                break;
            case StateType.Chase:
                currentState = chaseState;
                break;
            case StateType.Attack:
                currentState = attackState;
                break;
        }

        currentState.EnterState(this);
    }
}

public enum StateType
{
    Patrol,
    Chase,
    Attack,

}