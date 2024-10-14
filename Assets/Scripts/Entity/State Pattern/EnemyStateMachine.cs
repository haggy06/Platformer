using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [Header("Chase Setting")]
    [SerializeField, Tooltip("추적 시작 감지 콜라이어. 비워둘 시 Chase 상태가 되지 않는다.")]
    private TriggerEvent chaseONTrigger;
    [SerializeField, Tooltip("추적 종료 감지 콜라이어. 비워둘 시 Chase 상태가 풀리지 않는다.")]
    private TriggerEvent chaseOFFTrigger;

    [Header("Attack Setting")]
    [SerializeField, Tooltip("공격 여부 감지 콜라이어. 비워둘 시 Attack 상태가 되지 않는다.")]
    private TriggerEvent attackTrigger;

    protected override void Awake()
    {
        base.Awake();

        if (chaseONTrigger != null)
        {
            chaseONTrigger.TriggerEnterEvent += DetectionStart;
            chaseONTrigger.TriggerExitEvent += DetectionFinish;
        }
        if (chaseOFFTrigger != null)
        {
            chaseOFFTrigger.TriggerEnterEvent += DetectionStart;
            chaseOFFTrigger.TriggerExitEvent += DetectionFinish;
        }
        if (attackTrigger != null)
        {
            attackTrigger.TriggerEnterEvent += DetectionStart;
            attackTrigger.TriggerExitEvent += DetectionFinish;
        }
    }

    private void DetectionStart(TriggerEvent triggerEvent, Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var entity) && entity.EntityType == targetType) // 타겟의 히트박스 감지 시작 시
        {
            if (triggerEvent == attackTrigger) // 공격 애리어 입장 시
            {
                if (Target == collision.transform) // 추적하던 개체였을 경우
                {
                    ChangeState(StateType.Chase);
                }
            }
            else if (triggerEvent == chaseONTrigger) // 추적 시작 애리어 입장 시
            {
                if (Target != null) // 타겟이 지정되어 있지 않을 경우
                {
                    Target = collision.transform;
                    ChangeState(StateType.Chase);
                }
            }
        }
    }
    private void DetectionFinish(TriggerEvent triggerEvent, Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable entity) && entity.EntityType == targetType) // 타겟의 히트박스 감지 종료 시
        {
            if (Target == collision.transform) // 추적하던 개체였을 경우
            {
                if (triggerEvent == attackTrigger) // 공격 애리어 탈출 시
                {
                    currentState.TaskFinished += DelayedChangeState;
                }
                else if (triggerEvent == chaseOFFTrigger) // 추적 종료 애리어 입장 시
                {
                    ChangeState(StateType.Patrol);
                    chaseONTrigger.enabled = false;
                    Invoke("ChaseColliderBlink", 0.01f);
                }
            }
        }
    }
    private void ChaseColliderBlink()
    {
        chaseONTrigger.enabled = true;
    }

    private StateType delayedState;
    private void DelayedChangeState()
    {
        ChangeState(delayedState);
    }
}