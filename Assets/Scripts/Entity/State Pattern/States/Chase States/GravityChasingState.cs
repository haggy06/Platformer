using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChasingState : MoveState
{
    [SerializeField]
    private bool jumpWhenCliff;
    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);


    }
    public override void OnFixedUpdate(StateMachine stateMachine)
    {
        base.OnFixedUpdate(stateMachine);

        gravityEntity.Move(stateMachine.Target.position.x > transform.position.x ? 1 : -1, 0);
        if (jumpWhenCliff && (!Physics2D.OverlapCircle(new Vector2(gravityEntity.LookDir ? gravityEntity.Col.bounds.max.x : gravityEntity.Col.bounds.min.x, gravityEntity.Col.bounds.min.y), 0.1f, 1 << (int)LAYER.Ground) || // 이동 방향 발밑에 땅이 없거나
            Physics2D.OverlapCircle(new Vector2(gravityEntity.LookDir ? gravityEntity.Col.bounds.max.x : gravityEntity.Col.bounds.min.x, gravityEntity.Col.bounds.center.y), 0.1f, 1 << (int)LAYER.Ground)))
        {
            gravityEntity.Jump();
        }
    }
}
