using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GravityPrawlingState : MoveState
{
    [SerializeField, Tooltip("얼마나 자주 멈출지 간격.")]
    private Vector2Int stopTimeRange = Vector2Int.one;

    [SerializeField, Tooltip("얼마나 오래 멈출지 간격.")]
    private Vector2Int stopSpanRange = Vector2Int.one;

    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);

        InitTimes();
        if (!gravityMove.IsFrontGround || gravityMove.IsFrontWall) // 앞이 막다른 길일 경우
            gravityMove.Turn();


        gravityMove.CliffArriveEvent += Turn;
        gravityMove.WallArriveEvent += Turn;
    }
    public override void ExitState(StateMachine stateMachine)
    {
        base.ExitState(stateMachine);

        gravityMove.CliffArriveEvent -= Turn;
        gravityMove.WallArriveEvent -= Turn;
    }

    private void Turn()
    {
        print("Turn");
        gravityMove.Turn(); // 반대 방향으로 이동
    }

    [SerializeField]
    private float timer, curStopTime, curStopSpan;
    public override void OnFixedUpdate(StateMachine stateMachine)
    {
        #region _Prawl Logic_
        timer += Time.fixedDeltaTime;
        if (timer >= curStopTime)
        {
            gravityMove.Move(0, 0);
            if (timer >= curStopTime + curStopSpan)
            {
                InitTimes();
            }
        }
        #endregion
    }

    private void InitTimes()
    {
        timer = 0f;
        curStopTime = Random.Range(stopTimeRange.x, stopTimeRange.y);
        curStopSpan = Random.Range(stopSpanRange.x, stopSpanRange.y);
    }
}
