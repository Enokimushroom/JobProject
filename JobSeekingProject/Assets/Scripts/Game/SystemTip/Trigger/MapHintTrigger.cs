using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHintTrigger : TriggerBase
{
    private bool hintDone;
    [SerializeField] private string mapDes;
    [SerializeField] private string mapName;

    public override void Action()
    {
        if (!hintDone)
        {
            hintDone = true;
            UIMgr.Instance.MapHint(mapDes, mapName);
        }
    }

}
