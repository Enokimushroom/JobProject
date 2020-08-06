using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDeployerNoDamage : Deployer
{
    public override void DeploySkill()
    {
        //执行影响算法
        ImpactTargets();

        //判断远程近程用不同的接口实现不同的方法
        MoveMode();

        //改名区分Buffer
        gameObject.name = SkillData.skillName;

        //非播放完立刻销毁而是有条件
        CheckSelfInterruption();
    }

    public override void Destroy()
    {
        if (SkillData.canSelfInterrupt && SkillData.animationName != string.Empty)
        {
            SkillData.owner.GetComponent<Animator>().SetBool(SkillData.animationName, false);
        }
        base.Destroy();
    }
}
