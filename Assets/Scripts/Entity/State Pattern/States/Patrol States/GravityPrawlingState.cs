using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GravityPrawlingState : MoveState
{
    [SerializeField, Tooltip("�󸶳� ���� ������ ����.")]
    private Vector2Int stopTimeRange = Vector2Int.one;

    [SerializeField, Tooltip("�󸶳� ���� ������ ����.")]
    private Vector2Int stopSpanRange = Vector2Int.one;

    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);

        InitTimes();
        if (!gravityMove.IsFrontGround || gravityMove.IsFrontWall) // ���� ���ٸ� ���� ���
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
        gravityMove.Turn(); // �ݴ� �������� �̵�
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
