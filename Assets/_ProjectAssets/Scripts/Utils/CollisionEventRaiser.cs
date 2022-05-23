using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyCollisionEvent : UnityEvent<Collision2D>
{

}

public class CollisionEventRaiser : MonoBehaviour
{
    public MyCollisionEvent OnCollision2DEnter;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision2DEnter?.Invoke(collision);
    }
}
