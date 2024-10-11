using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMoveable
{
    public float MoveSpeed { get; set; }
    public float JumpPower { get; set; }

    public MoveData MoveData { get; }

    public void Move(Vector2 move);
    public void Stop();
    public void Turn(bool rightDirection);

    public void Jump();
}
