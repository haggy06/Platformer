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
    [SerializeField, Tooltip("���� = true, ���� = false")]
    protected bool lookDir = true;
    public Vector2 MoveDir => moveDir;
    public bool LookDir => lookDir;

    protected int moveDirX = 0;
    protected int moveDirY = 0;

    public BoxCollider2D Col { get; private set; }
    public Rigidbody2D Rigid2D { get; private set; }
    protected virtual void Awake()
    {
        Col = GetComponent<BoxCollider2D>();
        Rigid2D = GetComponentInParent<Rigidbody2D>();
    }

    [SerializeField]
    protected bool isGround = false;
    public bool IsGround => isGround;

    [SerializeField]
    private bool isFrontGround = false, isFrontWall = false;
    public bool IsFrontGround => isFrontGround;
    public bool IsFrontWall => isFrontWall;
    protected virtual void FixedUpdate()
    {
        #region _Ground Check_
        Vector2 footPosition = new Vector2(Col.bounds.center.x, Col.bounds.min.y - 0.1f);
        Vector2 offset = new Vector2((Col.bounds.size.x / 2f) - 0.01f, 0f); // ���� �پ��� �� �����Ǵ� �� ����
        Collider2D contactedGround = Physics2D.OverlapArea(footPosition - offset, footPosition + offset, 1 << (int)LAYER.Ground);

        bool isGround_New = contactedGround;
        if (isGround != isGround_New)
        {
            isGround = isGround_New;
            if (isGround)
                EventInvoke(EventType.Landing);
        }

        if (contactedGround) // ������ ���� ���� ���
        {
            if (contactedGround.TryGetComponent<GroundData>(out GroundData groundData)) // ���� GroundData ���� ���� ��
                moveData = groundData.MoveData;
            else // ���� ���� ��
                moveData = MoveData.defaultMove; // GroundData�� ������ ��� ������Ʈ �߰� �� ����
        }
        else // ������ ���� ���� ���
        {
            moveData = MoveData.airMove;
        }
        #endregion

        #region _Terrain Check_
        float xPos = lookDir ? Col.bounds.max.x : Col.bounds.min.x;
        if (CliffArriveEvent != null)
        {
            bool curFrontGround = Physics2D.OverlapPoint(new Vector2(xPos, Col.bounds.min.y - 0.1f), 1 << (int)LAYER.Ground);
            if (isFrontGround && !curFrontGround) // ���� �տ� �ٴ��� �־��µ� ���� �ٴ��� ���� ���
            {
                CliffArriveEvent.Invoke();
            }

            isFrontGround = curFrontGround;
        }

        if (WallArriveEvent != null)
        {
            float xOffset = lookDir ? 0.1f : -0.1f;
            bool curFrontWall = Physics2D.OverlapArea(new Vector2(xPos + xOffset, Col.bounds.min.y + 0.1f), new Vector2(xPos + xOffset, Col.bounds.max.y - 0.1f), 1 << (int)LAYER.Ground);
            if (!isFrontWall && curFrontWall) // ���� �տ� ���� �����µ� ���� ���� ���� ���
            {
                WallArriveEvent.Invoke();
            }

            isFrontWall = curFrontWall;
        }
        #endregion

        Move();
    }
    public event System.Action CliffArriveEvent;
    public event System.Action WallArriveEvent;

    protected abstract void Move();
    public void Move(int moveDirX, int moveDirY)
    {
        this.moveDirX = moveDirX;
        this.moveDirY = moveDirY;
    }
    public abstract void Turn();

    public abstract bool Jump(bool ignoreGround = false);
    public abstract bool JumpCancle();

    public void AddForce(Vector2 force)
    {
        Rigid2D.velocity = force;
    }

    protected void EventInvoke(EventType eventType)
    {
        switch(eventType)
        {
            case EventType.Turn:
                TurnEvent?.Invoke(lookDir);
                break;
            case EventType.Break:
                BreakEvent?.Invoke();
                break;
            case EventType.Step: 
                StepEvent?.Invoke();
                break;

            case EventType.Jump:
                JumpEvent?.Invoke();
                break;
            case EventType.Landing:
                LandingEvent?.Invoke();
                break;
        }
    }

    public event System.Action<bool> TurnEvent;
    public event System.Action BreakEvent;
    public event System.Action StepEvent;

    public event System.Action JumpEvent;
    public event System.Action LandingEvent;

    public enum EventType
    {
        Turn,
        Break,
        Step,
        
        Jump,
        Landing,

    }
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