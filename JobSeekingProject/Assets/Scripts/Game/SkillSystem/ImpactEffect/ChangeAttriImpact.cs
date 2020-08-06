using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAttriImpact : IImpactEffect
{
    public void Execute(Deployer deployer)
    {
        PlayerStatus.Instance.ChangeAttri(deployer.SkillData.info, deployer.SkillData.delta);
    }
}
