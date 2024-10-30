using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class GravityMove : MoveBase
{
    protected override void Move()
    {
        float goalSpeed = moveInfo.moveSpeed * moveData.moveSpeedRatio;
        Vector2 velo = Rigid2D.velocity;

        if (moveDirX == 0) // 정지일 경우
        {
            if (moveDir != Vector2.zero) // 움직이고 있었을 경우
            {
                moveDir = Vector2.zero;
                EventInvoke(EventType.Break);
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
                EventInvoke(EventType.Step);
            }

            if (moveDirX > 0) // 우측 이동일 경우
            {
                if (!lookDir) // 왼쪽을 보고 있었을 경우
                {
                    lookDir = true;
                    EventInvoke(EventType.Turn);
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
                    EventInvoke(EventType.Turn);
                }

                if (velo.x >= -goalSpeed) // 목표 속도에 미치지 못하거나 같을 경우
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), -goalSpeed, velo.x); // 가속
                else // 목표 속도를 넘었을 경우
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, -goalSpeed); // 감속
            }
        }

        #region _Conveyer Logic_
        velo += moveData.defaultSpeed;
        #endregion

        Rigid2D.velocity = velo;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Turn()
    {
        Move(LookDir ? -1 : 1, 0);
    }

    public override bool Jump(bool ignoreGround = false)
    {
        if (isGround || ignoreGround) // 착지해 있거나 착지 무시일 때
        {
            EventInvoke(EventType.Jump);
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

public enum TAG
{
    Untagged,
    Respawn,
    Finish,
    EditorOnly,
    MainCamera,
    Player,
    GameController,

}