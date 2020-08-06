using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHintTrigger : MonoBehaviour
{
    private bool hintDone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!hintDone)
        {
            hintDone = true;
            UIMgr.Instance.MapHint("衰落的小镇", "德莱茅斯");
        }
    }
}
