using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRemainMode : IMoveMode
{
    public void Excute(Deployer deployer)
    {
        deployer.transform.SetParent(deployer.SkillData.owner.transform);
    }
}
