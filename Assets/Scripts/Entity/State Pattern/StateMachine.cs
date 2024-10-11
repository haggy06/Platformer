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

    protected virtual void Awake()
    {
        ResetState(currentStateType);
    }
    protected virtual void Update()
    {
        currentState.OnUpdate();
    }
    protected virtual void FixedUpdate()
    {
        currentState.OnFixedUpdate();
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

        currentState.EnterState();
    }
    public void ChangeState(StateType newStateType)
    {
        currentState.ExitState();

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

        currentState.EnterState();
    }
}

public enum StateType
{
    Patrol,
    Chase,
    Attack,

}