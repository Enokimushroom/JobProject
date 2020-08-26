using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundIC : IInterruptCondition
{
    public bool Check(Deployer deployer)
    {
        return PlayerStatus.Instance.OnGround;
    }

    public void OnFinish(Deployer deployer)
    {
        MusicMgr.Instance.PlaySound("QuakeImpactAudio", false);
    }
}
