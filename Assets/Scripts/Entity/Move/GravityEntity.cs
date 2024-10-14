using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class GravityEntity : MoveBase
{
    [SerializeField]
    protected bool isGround = false;
    public bool IsGround => isGround;
    private void FixedUpdate()
    {
        #region _Ground Check_
        Vector2 footPosition = new Vector2(Col.bounds.center.x, Col.bounds.min.y - 0.1f);
        Vector2 offset = new Vector2((Col.bounds.size.x / 2f) - 0.01f, 0f); // ���� �پ��� �� �����Ǵ� �� ����
        Collider2D contactedGround = Physics2D.OverlapArea(footPosition - offset, footPosition + offset, 1 << (int)LAYER.Ground);

        isGround = contactedGround;

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
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void MaintainMove()
    {
        Move(LookDir ? 1 : -1, 0);
    }
    public override void Move(int moveDirX, int moveDirY)
    {
        //Debug.Log(moveData.driftTime);
        float goalSpeed = moveInfo.moveSpeed * moveData.moveSpeedRatio;
        Vector2 velo = Rigid2D.velocity;

        #region _Move Logic_
        if (moveDirX == 0) // ������ ���
        {
            if (moveDir != Vector2.zero) // �����̰� �־��� ���
            {
                moveDir = Vector2.zero;
                BreakEvent.Invoke();
            }

            if (velo.x > 0f) // �������� �̵� ���̾��� ���
                velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), 0f, velo.x); // ����
            else // �������� �̵� ���̾��� ���
                velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, 0f); // ����
        }
        else // �̵��� ���
        {
            if (moveDir == Vector2.zero) // �����̰� ���� �ʾ��� ���
            {
                moveDir = new Vector2(moveDirX, moveDirY);
                StepEvent.Invoke();
            }

            if (moveDirX > 0) // ���� �̵��� ���
            {
                if (!lookDir) // ������ ���� �־��� ���
                {
                    lookDir = true;
                    TurnEvent.Invoke(lookDir);
                }

                if (velo.x <= goalSpeed) // ��ǥ �ӵ��� ��ġ�� ���ϰų� ���� ���
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, goalSpeed); // ����
                else // ��ǥ �ӵ��� �Ѿ��� ���
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), goalSpeed, velo.x); // ����
            }
            else // ���� �̵��� ���
            {
                if (lookDir) // �������� ���� �־��� ���
                {
                    lookDir = false;
                    TurnEvent.Invoke(lookDir);
                }

                if (velo.x >= -goalSpeed) // ��ǥ �ӵ��� ��ġ�� ���ϰų� ���� ���
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), -goalSpeed, velo.x); // ����
                else // ��ǥ �ӵ��� �Ѿ��� ���
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, -goalSpeed); // ����
            }
        }
        #endregion

        #region _Conveyer Logic_
        velo += moveData.defaultSpeed;
        #endregion

        Rigid2D.velocity = velo;
    }
    public override bool Jump(bool ignoreGround = false)
    {
        if (isGround || ignoreGround) // ������ �ְų� ���� ������ ��
        {
            JumpEvent.Invoke();
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, moveInfo.jumpPower * moveData.jumpPowerRatio);

            return true;
        }
        else
            return false;
    }
    public override bool JumpCancle()
    {
        float jumpCancleVelo = moveInfo.jumpPower * moveInfo.jumpCancleRatio * moveData.jumpPowerRatio;
        if (Rigid2D.velocity.y >= jumpCancleVelo) // ���� ĵ�� �ӵ����� ������ ��� ���̾��� ���
        {
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, jumpCancleVelo);

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
    PhysicalBox,
    Censor,

}