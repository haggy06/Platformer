using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void Act(Controller controller);
}
