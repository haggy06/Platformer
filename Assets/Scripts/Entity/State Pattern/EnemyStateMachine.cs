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
    [SerializeField, Tooltip("���� ���� ���� �ݶ��̾�. ����� �� Chase ���°� ���� �ʴ´�.")]
    private Collider2D chaseONCollider;
    [SerializeField, Tooltip("���� ���� ���� �ݶ��̾�. ����� �� Chase ���°� Ǯ���� �ʴ´�.")]
    private Collider2D chaseOFFCollider;

    [Header("Attack Setting")]
    [SerializeField, Tooltip("���� ���� ���� �ݶ��̾�. ����� �� Attack ���°� ���� �ʴ´�.")]
    private Collider2D attackCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var entity) && entity.EntityType == targetType) // Ÿ���� ��Ʈ�ڽ� ���� ���� ��
        {
            if (attackCollider.IsTouching(collision)) // ���� �ָ��� ���� ��
            {
                if (targetTransform == collision.transform) // �����ϴ� ��ü���� ���
                {
                    ChangeState(StateType.Chase);
                }
            }
            else if (chaseONCollider.IsTouching(collision)) // ���� ���� �ָ��� ���� ��
            {
                if (targetTransform != null) // Ÿ���� �����Ǿ� ���� ���� ���
                {
                    targetTransform = collision.transform;
                    ChangeState(StateType.Chase);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable entity) && entity.EntityType == targetType) // Ÿ���� ��Ʈ�ڽ� ���� ���� ��
        {
            if (targetTransform == collision.transform) // �����ϴ� ��ü���� ���
            {
                if (attackCollider.IsTouching(collision)) // ���� �ָ��� Ż�� ��
                {
                    currentState.TaskFinished += DelayedChangeState;
                }
                else if (chaseOFFCollider.IsTouching(collision)) // ���� ���� �ָ��� ���� ��
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