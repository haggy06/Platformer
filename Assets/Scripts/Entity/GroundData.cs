using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundData : MonoBehaviour
{
    [Header("Move Data")]
    [SerializeField]
    private GroundType groundType = GroundType.Normal;

    [Space(5), SerializeField, Range(0f, 1f)]
    public float moveSpeedRatio;
    [SerializeField, Range(0f, 1f)]
    public float jumpPowerRatio;

    [Space(5), SerializeField, Range(0f, 1f)]
    public float friction;

    [Space(5), SerializeField, Tooltip("우측 상단 기준 속도")]
    public Vector2 conveyorSpeed = Vector2.zero;


    [Header("Move Sound")]
    [SerializeField]
    private AudioClip walkSound;
    [SerializeField]
    private AudioClip jumpSound;
}

public enum GroundType
{
    Normal,
    Slime
}

[System.Serializable]
public struct MoveInfo
{
    [Range(0f, 1f)]
    public float moveSpeedRatio;
    [Range(0f, 1f)]
    public float jumpPowerRatio;

    [Space(5), Range(0f, 1f)]
    public float friction;

    public MoveInfo(float moveSpeedRatio, float jumpPowerRatio, float friction)
    {
        this.moveSpeedRatio = moveSpeedRatio;
        this.jumpPowerRatio = jumpPowerRatio;
        this.friction = friction;
    }

    public static MoveInfo defaultInfo = new MoveInfo(1f, 1f, 1f);
}