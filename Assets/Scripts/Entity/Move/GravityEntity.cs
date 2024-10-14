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
        Vector2 offset = new Vector2((Col.bounds.size.x / 2f) - 0.01f, 0f); // 벽에 붙었을 때 감지되는 걸 방지
        Collider2D contactedGround = Physics2D.OverlapArea(footPosition - offset, footPosition + offset, 1 << (int)LAYER.Ground);

        isGround = contactedGround;

        if (contactedGround) // 착지한 땅이 있을 경우
        {
            if (contactedGround.TryGetComponent<GroundData>(out GroundData groundData)) // 땅의 GroundData 참조 성공 시
                moveData = groundData.MoveData;
            else // 참조 실패 시
                moveData = MoveData.defaultMove; // GroundData가 없었을 경우 컴포넌트 추가 후 참조
        }
        else // 착지한 땅이 없을 경우
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
        if (moveDirX == 0) // 정지일 경우
        {
            if (moveDir != Vector2.zero) // 움직이고 있었을 경우
            {
                moveDir = Vector2.zero;
                BreakEvent.Invoke();
            }

            if (velo.x > 0f) // 우측으로 이동 중이었을 경우
                velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), 0f, velo.x); // 감속
            else // 좌측으로 이동 중이었을 경우
                velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, 0f); // 감속
        }
        else // 이동일 경우
        {
            if (moveDir == Vector2.zero) // 움직이고 있지 않았을 경우
            {
                moveDir = new Vector2(moveDirX, moveDirY);
                StepEvent.Invoke();
            }

            if (moveDirX > 0) // 우측 이동일 경우
            {
                if (!lookDir) // 왼쪽을 보고 있었을 경우
                {
                    lookDir = true;
                    TurnEvent.Invoke(lookDir);
                }

                if (velo.x <= goalSpeed) // 목표 속도에 미치지 못하거나 같을 경우
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, goalSpeed); // 가속
                else // 목표 속도를 넘었을 경우
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), goalSpeed, velo.x); // 감속
            }
            else // 좌측 이동일 경우
            {
                if (lookDir) // 오른쪽을 보고 있었을 경우
                {
                    lookDir = false;
                    TurnEvent.Invoke(lookDir);
                }

                if (velo.x >= -goalSpeed) // 목표 속도에 미치지 못하거나 같을 경우
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), -goalSpeed, velo.x); // 가속
                else // 목표 속도를 넘었을 경우
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, -goalSpeed); // 감속
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
        if (isGround || ignoreGround) // 착지해 있거나 착지 무시일 때
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
        if (Rigid2D.velocity.y >= jumpCancleVelo) // 점프 캔슬 속도보다 빠르게 상승 중이었을 경우
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