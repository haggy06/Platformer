using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityEntity))]
public class MoveState : State
{
    [Header("Move Setting")]
    [SerializeField]
    protected bool changeMoveInfo = true;
    [SerializeField]
    protected MoveInfo moveInfo;
    protected GravityEntity gravityEntity;
    protected virtual void Awake()
    {
        gravityEntity = GetComponent<GravityEntity>();
    }

    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);

        if (changeMoveInfo)
            gravityEntity.moveInfo = moveInfo;
    }
}
