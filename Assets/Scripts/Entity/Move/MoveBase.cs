using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class MoveBase : MonoBehaviour
{
    [Header("Move Setting")]
    public MoveInfo moveInfo = new MoveInfo(5f, 5f, 0.1f);

    [Header("Move Status")]
    [SerializeField]
    protected MoveData moveData = new MoveData(GroundType.Normal, 1f, 1f, 0.8f, Vector2.zero);
    [SerializeField]
    protected Vector2 moveDir = Vector2.zero;
    [SerializeField, Tooltip("¿ìÃø = true, ÁÂÃø = false")]
    protected bool lookDir = true;
    public Vector2 MoveDir => moveDir;
    public bool LookDir => lookDir;

    public BoxCollider2D Col { get; private set; }
    public Rigidbody2D Rigid2D { get; private set; }
    protected virtual void Awake()
    {
        Col = GetComponent<BoxCollider2D>();
        Rigid2D = GetComponentInParent<Rigidbody2D>();
    }
    public abstract void MaintainMove();
    public abstract void Move(int moveDirX, int moveDirY);
    public abstract bool Jump(bool ignoreGround = false);
    public abstract bool JumpCancle();
}

[System.Serializable]
public struct MoveInfo
{
    public float moveSpeed;
    public float jumpPower;

    [Range(0f, 1f)]
    public float jumpCancleRatio;

    public MoveInfo(float moveSpeed, float jumpPower, float jumpCancleRatio)
    {
        this.moveSpeed = moveSpeed;
        this.jumpPower = jumpPower;
        this.jumpCancleRatio = jumpCancleRatio;
    }
}