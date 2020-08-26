using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TriggerBase : MonoBehaviour
{
    protected GameObject collision;
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.collision = collision.gameObject;
            Action();
        }
    }

    public abstract void Action();
    
}
