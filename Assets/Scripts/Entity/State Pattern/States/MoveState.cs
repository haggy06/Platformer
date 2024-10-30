using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityMove))]
public class MoveState : State
{
    [Header("Move Setting")]
    [SerializeField]
    protected bool changeMoveInfo = true;
    [SerializeField]
    protected MoveInfo moveInfo;
    protected GravityMove gravityMove;
    protected virtual void Awake()
    {
        gravityMove = GetComponent<GravityMove>();
    }

    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);

        if (changeMoveInfo)
            gravityMove.moveInfo = moveInfo;
    }
}
