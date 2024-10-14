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
        gravityEntity.Move(gravityEntity.LookDir ? 1 : -1, 0);
    }
    [SerializeField]
    private float timer, curStopTime, curStopSpan;
    public override void OnFixedUpdate(StateMachine stateMachine)
    {
        if (!Physics2D.OverlapCircle(new Vector2(gravityEntity.LookDir ? gravityEntity.Col.bounds.max.x : gravityEntity.Col.bounds.min.x, gravityEntity.Col.bounds.min.y), 0.1f, 1 << (int)LAYER.Ground) || // �̵� ���� �߹ؿ� ���� ���ų�
            Physics2D.OverlapCircle(new Vector2(gravityEntity.LookDir ? gravityEntity.Col.bounds.max.x : gravityEntity.Col.bounds.min.x, gravityEntity.Col.bounds.center.y), 0.1f, 1 << (int)LAYER.Ground)) // �̵� ���⿡ ���� ���� ���
        {
            if (gravityEntity.IsGround) // ������ ���� ���
                gravityEntity.Move(gravityEntity.LookDir ? -1 : 1, 0); // �ݴ� �������� �̵�
        }
        #region _Prawl Logic_
        timer += Time.fixedDeltaTime;
        if (timer >= curStopTime)
        {
            gravityEntity.Move(0, 0);
            if (timer >= curStopTime + curStopSpan)
            {
                InitTimes();
                gravityEntity.Move(gravityEntity.LookDir ? 1 : -1, 0);
            }
        }
        else
        {
            gravityEntity.MaintainMove();
        }
        #endregion
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(gravityEntity.LookDir ? gravityEntity.Col.bounds.max.x : gravityEntity.Col.bounds.min.x, gravityEntity.Col.bounds.min.y, 0f), 0.1f);
            Gizmos.DrawSphere(new Vector3(gravityEntity.LookDir ? gravityEntity.Col.bounds.max.x : gravityEntity.Col.bounds.min.x, gravityEntity.Col.bounds.center.y, 0f), 0.1f);
        }
    }

    private void InitTimes()
    {
        timer = 0f;
        curStopTime = Random.Range(stopTimeRange.x, stopTimeRange.y);
        curStopSpan = Random.Range(stopSpanRange.x, stopSpanRange.y);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
