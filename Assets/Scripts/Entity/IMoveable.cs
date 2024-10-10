using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMoveable
{
    public MoveData MoveData { get; protected set; }

    public void Move(Vector2 move);
    public void Stop();
    public void Turn(bool rightDirection);
}
