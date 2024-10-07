using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundData : MonoBehaviour
{
    [SerializeField]
    private MoveData moveData = new MoveData(GroundType.Normal, 1f, 1f, 0.06f, Vector2.zero);
    public MoveData MoveData => moveData;
}

public enum GroundType
{
    Normal,
    Slime,

    Air
}

[System.Serializable]
public class MoveData
{
    public GroundType groundType = GroundType.Normal;

    [Space(5), Range(0f, 1f)]
    public float moveSpeedRatio = 1f;
    [Range(0f, 1f)]
    public float jumpPowerRatio = 1f;

    [Space(5), Range(0.02f, 1f)]
    public float driftTime = 0.8f;

    [Space(5)]
    public Vector2 defaultSpeed = Vector2.zero;


    [Header("Sound")]
    public AudioClip walkSound;
    public AudioClip jumpSound;

    public MoveData(GroundType groundType, float moveSpeedRatio, float jumpPowerRatio, float driftTime, Vector2 defaultSpeed)
    {
        this.groundType = groundType;
        this.moveSpeedRatio = moveSpeedRatio; 
        this.jumpPowerRatio = jumpPowerRatio;
        this.driftTime = driftTime;

        this.defaultSpeed = defaultSpeed;
    }

    private static MoveData _defaltData = null;
    /// <summary> { 1f, 1f, 0.02f, "NormalWalk", "NormalJump" } </summary>
    public static MoveData defaultMove
    {
        get
        {
            if (_defaltData == null) // 기본 데이터가 지정되어 있지 않을 경우
            {
                _defaltData = new MoveData(GroundType.Normal, 1f, 1f, 0.06f, Vector2.zero);
                _defaltData.walkSound = ResourceLoader<AudioClip>.ResourceLoad(FolderName.Player, "NormalWalk");
                _defaltData.jumpSound = ResourceLoader<AudioClip>.ResourceLoad(FolderName.Player, "NormalJump");
            }

            return _defaltData;
        }
    }

    /// <summary> { 1f, 1f, 0.8f, Vector2.zero } </summary>
    public static readonly MoveData airMove = new MoveData(GroundType.Air, 1f, 1f, 0.8f, Vector2.zero);

    public override string ToString()
    {
        return ("{ " + groundType + ", " + moveSpeedRatio + ", " + jumpPowerRatio + ", " + driftTime + ", " + defaultSpeed + " }");
    }
}