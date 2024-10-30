using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TriggerEvent : MonoBehaviour
{
    public event System.Action<TriggerEvent, Collider2D> TriggerEnterEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (TriggerEnterEvent != null)
            TriggerEnterEvent.Invoke(this, collision);
    }

    /*
    public event System.Action<TriggerEvent, Collider2D> TriggerStayEvent;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TriggerStayEvent != null)
            TriggerStayEvent.Invoke(this, collision);
    }
    */

    public event System.Action<TriggerEvent, Collider2D> TriggerExitEvent;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TriggerExitEvent != null)
            TriggerExitEvent.Invoke(this, collision);
    }
}
