using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempRebornPosTrigger : TriggerBase
{
    public int index;

    public override void Action()
    {
        PlayerStatus.Instance.TempRebornPos = index;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }
}
