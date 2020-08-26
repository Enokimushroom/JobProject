using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LiftTrigger : TriggerBase
{
    private Lift lift;
    private BoxCollider2D col;

    private void Start()
    {
        lift = transform.parent.GetChild(1).GetComponent<Lift>();
        col = transform.GetComponent<BoxCollider2D>();
    }

    public override void Action()
    {
        Debug.Log("1");
        if (lift.up && !lift.moving)
        {
            lift.BackDown();
            col.offset = new Vector2(111, 13);
        }
        else if (lift.down && !lift.moving)
        {
            lift.BackUp();
            col.offset = new Vector2(100, -16);
        }
    }
}
