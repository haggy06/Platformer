using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public abstract class MoveBase : MonoBehaviour
{
    [Header("Move Setting")]
    public MoveInfo moveInfo = new MoveInfo(5f, 5f, 0.1f);

    [Header("Move Status")]
    [SerializeField]
    protected MoveData moveData = new MoveData(GroundType.Normal, 1f, 1f, 0.8f, Vector2.zero);
    [SerializeField]
    protected bool isMove = false;
    [SerializeField, Tooltip("¿ìÃø = true, ÁÂÃø = false")]
    protected bool lookDir = true;
    public bool IsMove => isMove;
    public bool LookDir => lookDir;

    protected BoxCollider2D col;
    protected Rigidbody2D rigid2D;
    protected virtual void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public abstract void Move(int moveDirX, int moveDirY);
    public abstract bool Jump(bool ignoreGround = false);
    public abstract bool JumpCancle();
}

[System.Serializable]
public struct MoveInfo
{
    public float moveSpeed;
    public float jumpPower;
    public float jumpCancleRatio;

    public MoveInfo(float moveSpeed, float jumpPower, float jumpCancleRatio)
    {
        this.moveSpeed = moveSpeed;
        this.jumpPower = jumpPower;
        this.jumpCancleRatio = jumpCancleRatio;
    }
}