using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField]
    private EntityType targetType;
    private Transform targetTransform = null;
    public Transform TargetTransform => targetTransform;

    [Header("Chase Setting")]
    [SerializeField, Tooltip("추적 시작 감지 콜라이어. 비워둘 시 Chase 상태가 되지 않는다.")]
    private Collider2D chaseONCollider;
    [SerializeField, Tooltip("추적 종료 감지 콜라이어. 비워둘 시 Chase 상태가 풀리지 않는다.")]
    private Collider2D chaseOFFCollider;

    [Header("Attack Setting")]
    [SerializeField, Tooltip("공격 여부 감지 콜라이어. 비워둘 시 Attack 상태가 되지 않는다.")]
    private Collider2D attackCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var entity) && entity.EntityType == targetType) // 타겟의 히트박스 감지 시작 시
        {
            if (attackCollider.IsTouching(collision)) // 공격 애리어 입장 시
            {
                if (targetTransform == collision.transform) // 추적하던 개체였을 경우
                {
                    ChangeState(StateType.Chase);
                }
            }
            else if (chaseONCollider.IsTouching(collision)) // 추적 시작 애리어 입장 시
            {
                if (targetTransform != null) // 타겟이 지정되어 있지 않을 경우
                {
                    targetTransform = collision.transform;
                    ChangeState(StateType.Chase);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable entity) && entity.EntityType == targetType) // 타겟의 히트박스 감지 종료 시
        {
            if (targetTransform == collision.transform) // 추적하던 개체였을 경우
            {
                if (attackCollider.IsTouching(collision)) // 공격 애리어 탈출 시
                {
                    currentState.TaskFinished += DelayedChangeState;
                }
                else if (chaseOFFCollider.IsTouching(collision)) // 추적 종료 애리어 입장 시
                {
                    ChangeState(StateType.Patrol);
                    chaseONCollider.enabled = false;
                    Invoke("ChaseColliderBlink", 0.01f);
                }
            }
        }
    }
    private void ChaseColliderBlink()
    {
        chaseONCollider.enabled = true;
    }

    private StateType delayedState;
    private void DelayedChangeState()
    {
        ChangeState(delayedState);
    }
}