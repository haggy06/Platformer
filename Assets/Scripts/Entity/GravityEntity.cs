using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class GravityEntity : MonoBehaviour
{
    [Header("Move Setting")]
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float jumpCancleRatio = 0.1f;

    [Header("Move Status")]
    [SerializeField]
    protected MoveData moveData = MoveData.AirMove;
    [SerializeField]
    protected bool isGround = false;
    [SerializeField]
    protected bool isMove = false;
    [SerializeField, Tooltip("���� = true, ���� = false")]
    protected bool lookDir = true;
    public bool IsGround => isGround;
    public bool IsMove => isMove;
    public bool LookDir => lookDir;

    protected BoxCollider2D col;
    protected Rigidbody2D rigid2D;
    protected void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        #region _Ground Check_
        Vector2 footPosition = new Vector2(col.bounds.center.x, col.bounds.min.y - 0.1f);
        Vector2 offset = new Vector2((col.bounds.size.x / 2f) - 0.01f, 0f); // ���� �پ��� �� �����Ǵ� �� ����
        Collider2D contactedGround = Physics2D.OverlapArea(footPosition - offset, footPosition + offset, 1 << (int)LAYER.Ground);

        isGround = contactedGround;

        if (contactedGround) // ������ ���� ���� ���
        {
            if (contactedGround.TryGetComponent<GroundData>(out GroundData groundData)) // ���� GroundData ���� ���� ��
                moveData = groundData.MoveData;
            else // ���� ���� ��
                moveData = MoveData.DefaultMove; // GroundData�� ������ ��� ������Ʈ �߰� �� ����
        }
        else // ������ ���� ���� ���
        {
            moveData = MoveData.AirMove;

            Debug.LogWarning(moveData.driftTime + ", " + MoveData.AirMove.driftTime);
        }
        #endregion
    }

    public virtual void Move(int moveDir)
    {
        Debug.Log(moveData.driftTime);
        float goalSpeed = moveSpeed * moveData.moveSpeedRatio;
        Vector2 velo = rigid2D.velocity;

        #region _Move Logic_
        if (moveDir == 0) // ������ ���
        {
            if (isMove) // �����̰� �־��� ���
            {
                isMove = false;
                BreakEvent.Invoke();
            }

            if (velo.x > 0f) // �������� �̵� ���̾��� ���
            {
                velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), 0f, velo.x);
            }
            else // �������� �̵� ���̾��� ���
            {
                velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, 0f);
            }
        }
        else // �̵��� ���
        {
            if (!isMove) // �����̰� ���� �ʾ��� ���
            {
                isMove = true;
                StepEvent.Invoke();
            }

            if (moveDir > 0) // ���� �̵��� ���
            {
                if (!lookDir) // ������ ���� �־��� ���
                {
                    lookDir = true;
                    TurnEvent.Invoke(lookDir);
                }

                if (velo.x <= goalSpeed) // ��ǥ �ӵ��� ��ġ�� ���ϰų� ���� ���
                {
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, goalSpeed); // ����
                }
                else // ��ǥ �ӵ��� �Ѿ��� ���
                {
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), goalSpeed, velo.x); // ����
                }
            }
            else // ���� �̵��� ���
            {
                if (lookDir) // �������� ���� �־��� ���
                {
                    lookDir = false;
                    TurnEvent.Invoke(lookDir);
                }

                if (velo.x >= -goalSpeed) // ��ǥ �ӵ��� ��ġ�� ���ϰų� ���� ���
                {
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), -goalSpeed, velo.x); // ����
                }
                else // ��ǥ �ӵ��� �Ѿ��� ���
                {
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, -goalSpeed); // ����
                }
            }
        }
        #endregion

        #region _Conveyer Logic_
        velo += moveData.defaultSpeed;
        #endregion

        rigid2D.velocity = velo;
    }
    public virtual bool Jump(bool ignoreGround = false)
    {
        if (isGround || ignoreGround) // ������ �ְų� ���� ������ ��
        {
            JumpEvent.Invoke();
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpPower * moveData.jumpPowerRatio);

            return true;
        }
        else
            return false;
    }
    public virtual bool JumpCancle()
    {
        float jumpCancleVelo = jumpPower * jumpCancleRatio * moveData.jumpPowerRatio;
        if (rigid2D.velocity.y >= jumpCancleVelo) // ���� ĵ�� �ӵ����� ������ ��� ���̾��� ���
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpCancleVelo);

            return true;
        }
        else
            return false;
    }

    public event System.Action<bool> TurnEvent = (_) => { };
    public event System.Action BreakEvent = () => { };
    public event System.Action StepEvent = () => { };

    public event System.Action JumpEvent = () => { };
}

public enum LAYER
{
    Default,
    TtansParentFX,
    Ignore_Raycast,
    Ground,
    Water,
    UI,

}