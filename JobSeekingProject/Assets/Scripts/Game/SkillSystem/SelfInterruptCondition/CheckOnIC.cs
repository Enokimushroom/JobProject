using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnIC : IInterruptCondition
{
    public bool Check(Deployer deployer)
    {
        return !SkillMgr.Instance.badgeSkill.ContainsKey(deployer.SkillData.skillID);
    }

    public void OnFinish(Deployer deployer)
    {
        //如果是数值护符，此时已检测到护符不在装备状态，销毁前修正数值
        if (deployer.SkillData.changeAtr)
        {
            PlayerStatus.Instance.ChangeAttri(deployer.SkillData.info, -1 * deployer.SkillData.delta);
        }
    }
}
