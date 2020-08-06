using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWallIC : IInterruptCondition
{
    public bool Check(Deployer deployer)
    {
        return PlayerStatus.Instance.IsTouchingWall;
    }

    public void OnFinish(Deployer deployer)
    {

    }
}
