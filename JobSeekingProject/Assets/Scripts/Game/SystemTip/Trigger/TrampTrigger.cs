using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampTrigger : TriggerBase
{
    public override void Action()
    {
        Debug.Log("玩家踩到陷阱啦");
    }
}
