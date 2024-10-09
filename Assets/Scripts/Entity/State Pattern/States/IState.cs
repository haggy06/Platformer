using UnityEngine;

public interface IState
{
    public void StateStart(Controller controller);
    public void StateFinish(Controller controller);
}
