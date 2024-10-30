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

        if (moveDirX == 0) // ������ ���
        {
            if (moveDir != Vector2.zero) // �����̰� �־��� ���
            {
                moveDir = Vector2.zero;
                EventInvoke(EventType.Break);
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
                EventInvoke(EventType.Step);
            }

            if (moveDirX > 0) // ���� �̵��� ���
            {
                if (!lookDir) // ������ ���� �־��� ���
                {
                    lookDir = true;
                    EventInvoke(EventType.Turn);
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
                    EventInvoke(EventType.Turn);
                }

                if (velo.x >= -goalSpeed) // ��ǥ �ӵ��� ��ġ�� ���ϰų� ���� ���
                    velo.x = Mathf.Clamp(velo.x - (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), -goalSpeed, velo.x); // ����
                else // ��ǥ �ӵ��� �Ѿ��� ���
                    velo.x = Mathf.Clamp(velo.x + (goalSpeed / moveData.driftTime * Time.fixedDeltaTime), velo.x, -goalSpeed); // ����
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
        if (isGround || ignoreGround) // ������ �ְų� ���� ������ ��
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
        if (Rigid2D.velocity.y >= jumpCancleVelo) // ���� ĵ�� �ӵ����� ������ ��� ���̾��� ���
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