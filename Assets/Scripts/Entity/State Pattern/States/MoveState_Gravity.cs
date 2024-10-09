using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[RequireComponent(typeof(GravityEntity))]
public abstract class MoveState_Gravity : MonoBehaviour, IState
{
    [Header("Move State Setting")]
    [SerializeField]
    private bool ignoreCliff;

    [Space(5)]
    [SerializeField, Tooltip("�� �� �̵����� �� �̵��� �����Ǵ� �ð� (x ~ y)")]
    private Vector2 moveTimeSpan = new Vector2(1f, 2f);
    [SerializeField, Tooltip("�� �� �������� �� ������ �����Ǵ� �ð� (x ~ y)")]
    private Vector2 stopTimeSpan = new Vector2(1f, 2f);
    [SerializeField, Range(0f, 1f), Tooltip("���� �� ������ �ٲ� Ȯ��")]
    private float turnProb = 0f;


    private GravityEntity gravityEntity;

    public void StateStart(Controller controller)
    {

    }
    public void StateFinish(Controller controller)
    {
        
    }
}
