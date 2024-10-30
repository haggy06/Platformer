using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [Header("Chase Setting")]
    [SerializeField, Tooltip("���� ���� ���� �ݶ��̾�. ����� �� Chase ���°� ���� �ʴ´�.")]
    private TriggerEvent chaseONTrigger;
    [SerializeField, Tooltip("���� ���� ���� �ݶ��̾�. ����� �� Chase ���°� Ǯ���� �ʴ´�.")]
    private TriggerEvent chaseOFFTrigger;
    [SerializeField, Tooltip("��׷ΰ� Ǯ���� ���� ���� �������� �������� �־�� �ϴ� �ð�.")]
    private float targetingOffTime = 1f;

    [Header("Attack Setting")]
    [SerializeField, Tooltip("���� ���� ���� �ݶ��̾�. ����� �� Attack ���°� ���� �ʴ´�.")]
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
        if (collision.tag.Equals(targetTag.ToString())) // Ÿ�� ���� ���� ��
        {
            if (triggerEvent == attackTrigger) // ���� �ָ���� ��
            {
                if (Target == collision.transform) // �����ϴ� ��ü���� ���
                {
                    ChangeState(StateType.Attack);
                }
            }
            else if (triggerEvent == chaseONTrigger) // ���� ���� �ָ���� ��
            {
                if (Target == null) // Ÿ���� �����Ǿ� ���� ���� ���
                {
                    Target = collision.transform;
                    ChangeState(StateType.Chase);
                }
            }
            else if (triggerEvent == chaseOFFTrigger) // ���� ���� �ָ���� ��
            {
                StopCoroutine("TargetingCountdown");
            }
        }
    }
    private void DetectionFinish(TriggerEvent triggerEvent, Collider2D collision)
    {
        print("DetectionFinish");
        if (Target == collision.transform) // �����ϴ� ��ü ���� ���� ��
        {
            if (triggerEvent == attackTrigger) // ���� �ָ��� Ż�� ��
            {
                currentState.TaskFinishEvent += DelayedChangeState;
            }
            else if (triggerEvent == chaseOFFTrigger) // ���� ���� �ָ��� ���� ��
            {
                StartCoroutine("TargetingCountdown");
            }
        }
    }
    private IEnumerator TargetingCountdown()
    {
        yield return YieldReturn.WaitForSeconds(targetingOffTime);
        Debug.Log("��׷� ���� ����");

        ChangeState(StateType.Patrol);

        Target = null;

        chaseONTrigger.enabled = false;
        Invoke("ChaseColliderBlink", 0.01f);
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