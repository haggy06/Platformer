using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceTask : Task
{
    [SerializeField]
    protected Vector2 forcePower;
    [SerializeField]
    protected bool canForceOnlyGround = true;
    [SerializeField]
    protected Task taskWhenLanding;

    protected MoveBase move;
    public override void TaskStart(SequenceState sequence)
    {
        base.TaskStart(sequence);

        move = sequence.GetComponent<MoveBase>();
        if (move != null && (!canForceOnlyGround || move.IsGround))
        {
            move.AddForce(forcePower);
            move.LandingEvent += LandingTaskInvoke;
        }
        else
            TaskComplete();
        }

    public override void TaskComplete()
    {
        move.LandingEvent -= LandingTaskInvoke;
        base.TaskComplete();
    }

    private void LandingTaskInvoke()
    {
        if (taskWhenLanding != null)
            taskWhenLanding.TaskStart(sequence);

        TaskComplete();
    }
}
