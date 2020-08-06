using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDyingIC : IInterruptCondition
{
    public bool Check(Deployer deployer)
    {
        return !PlayerStatus.Instance.IsDying;
    }

    public void OnFinish(Deployer deployer)
    {
        if (deployer.SkillData.changeAtr)
            PlayerStatus.Instance.ChangeAttri(deployer.SkillData.info, -1 * deployer.SkillData.delta);
    }
}
