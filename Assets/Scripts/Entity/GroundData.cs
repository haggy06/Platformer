using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundData : MonoBehaviour
{
    [SerializeField]
    private MoveData moveData = new MoveData(1f, 1f, 1f, Vector2.zero);
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

    [Space(5), Range(0f, 1f)]
    public float friction = 1f;

    [Space(5)]
    public Vector2 defaultSpeed = Vector2.zero;


    [Header("Sound")]
    public AudioClip walkSound;
    public AudioClip jumpSound;

    public MoveData(float moveSpeedRatio, float jumpPowerRatio, float friction, Vector2 defaultSpeed)
    {
        this.moveSpeedRatio = moveSpeedRatio; 
        this.jumpPowerRatio = jumpPowerRatio;
        this.friction = friction;

        this.defaultSpeed = defaultSpeed;
    }

    private static MoveData _defaltData = null;
    public static MoveData DefaultMove
    {
        get
        {
            if (_defaltData == null) // 기본 데이터가 지정되어 있지 않을 경우
            {
                _defaltData = new MoveData(1f, 1f, 0.02f, Vector2.zero);
                _defaltData.walkSound = ResourceLoader<AudioClip>.ResourceLoad(FolderName.Player, "NormalWalk");
                _defaltData.jumpSound = ResourceLoader<AudioClip>.ResourceLoad(FolderName.Player, "NormalJump");
            }

            return _defaltData;
        }
    }
    public static MoveData AirMove = new MoveData(1f, 1f, 0.05f, Vector2.zero);
}