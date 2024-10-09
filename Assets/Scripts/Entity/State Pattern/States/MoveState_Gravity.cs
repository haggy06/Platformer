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
    [SerializeField, Tooltip("한 번 이동했을 때 이동이 유지되는 시간 (x ~ y)")]
    private Vector2 moveTimeSpan = new Vector2(1f, 2f);
    [SerializeField, Tooltip("한 번 정지했을 때 정지가 유지되는 시간 (x ~ y)")]
    private Vector2 stopTimeSpan = new Vector2(1f, 2f);
    [SerializeField, Range(0f, 1f), Tooltip("정지 후 방향을 바꿀 확률")]
    private float turnProb = 0f;


    private GravityEntity gravityEntity;

    public void StateStart(Controller controller)
    {

    }
    public void StateFinish(Controller controller)
    {
        
    }
}
