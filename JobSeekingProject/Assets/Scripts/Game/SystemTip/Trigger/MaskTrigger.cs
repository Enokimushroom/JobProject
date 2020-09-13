using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaskTrigger : TriggerBase
{
    public override void Action()
    {
        GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), 1.0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 1), 1.0f);
        }
    }
}
