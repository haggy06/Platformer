using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChasingState : MoveState
{
    [SerializeField]
    private TerrainDecisionType decisionWhenCliff = TerrainDecisionType.Stop;
    [SerializeField]
    private TerrainDecisionType decisionWhenWall = TerrainDecisionType.Stop;

    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);
        timer = 0f;

        if (decisionWhenCliff != TerrainDecisionType.Ignore)
            gravityMove.CliffArriveEvent += Jump;
        if (decisionWhenWall != TerrainDecisionType.Ignore)
            gravityMove.WallArriveEvent += Jump;
    }
    public override void ExitState(StateMachine stateMachine)
    {
        base.ExitState(stateMachine);

        if (decisionWhenCliff != TerrainDecisionType.Ignore)
            gravityMove.CliffArriveEvent -= Jump;
        if (decisionWhenWall != TerrainDecisionType.Ignore)
            gravityMove.WallArriveEvent -= Jump;
    }

    private void Jump()
    {
        
    }

    private float timer;
    private bool canMove = true;
    public override void OnFixedUpdate(StateMachine stateMachine)
    {
        base.OnFixedUpdate(stateMachine);
        canMove = true;

        timer += Time.fixedDeltaTime;
        if (timer >= 0.1f)
        {
            timer = 0f;
            gravityMove.Move(stateMachine.Target.position.x > transform.position.x ? 1 : -1, 0);
        }

        if (gravityMove.IsGround) // 착지해 있을 경우
        {
            if (!gravityMove.IsFrontGround) // 절벽일 경우
            {
                switch (decisionWhenCliff)
                {
                    case TerrainDecisionType.Stop:
                        canMove = false;
                        gravityMove.Move(0, 0);
                        break;

                    case TerrainDecisionType.Jump:
                        gravityMove.Jump();
                        break;

                    default: break;
                }
            }
            else if (gravityMove.IsFrontWall) // 막다른 길일 경우
            {
                switch (decisionWhenWall)
                {
                    case TerrainDecisionType.Stop:
                        canMove = false;
                        gravityMove.Move(0, 0);
                        break;

                    case TerrainDecisionType.Jump:
                        gravityMove.Jump();
                        break;

                    default: break;
                }
            }
            else
            {
                canMove = true;
            }
        }
    }
}

public enum TerrainDecisionType
{
    Ignore,
    Stop,
    Jump,

}