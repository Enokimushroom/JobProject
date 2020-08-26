using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmTrigger : TriggerBase
{
    [SerializeField] private string bgmName;

    public override void Action()
    {
        MusicMgr.Instance.PlayBGMusic(bgmName);
    }
}
