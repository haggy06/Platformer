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
        if (!currentState.CanFinishState) // 현재 상태를 종료할 수 없을 경우
        {
            currentState.TaskFinishEvent += () => ChangeState(newStateType); // 현재 상태가 종료 가능해질 때 이 메소드를 재호출한다.
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