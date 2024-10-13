using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityEntity))]
public class PrawlingState : State
{
    [SerializeField, Tooltip("얼마나 자주 멈출지 간격.")]
    private Vector2Int stopTimeRange = Vector2Int.one;

    [SerializeField, Tooltip("얼마나 오래 멈출지 간격.")]
    private Vector2Int stopTimeSpan = Vector2Int.one;

    private GravityEntity gravityEntity;
    private void Awake()
    {
        gravityEntity = GetComponent<GravityEntity>();
    }

    public override void EnterState()
    {
        base.EnterState();

        StartCoroutine("PrawlCoroutine");
    }
    public override void ExitState()
    {
        base.ExitState();

        StopCoroutine("PrawlCoroutine");
    }

    private IEnumerator PrawlCoroutine()
    {
        while (isActive)
        {
            gravityEntity.Move(gravityEntity.LookDir ? 1 : -1, 0); // 정면 이동

            yield return YieldReturn.WaitForSeconds(Random.Range(stopTimeRange.x, stopTimeRange.y));

            

            yield return YieldReturn.WaitForSeconds(Random.Range(stopTimeSpan.x, stopTimeSpan.y));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
