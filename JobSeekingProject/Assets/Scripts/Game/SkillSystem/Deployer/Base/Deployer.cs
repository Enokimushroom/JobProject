using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 技能释放器
/// </summary>
public abstract class Deployer :MonoBehaviour
{
    private SkillData skillData;
    public SkillData SkillData
    {
        get { return skillData; }
        set { skillData = value; InitDeployer(); }
    }

    /// <summary>
    /// 影响算法对象
    /// </summary>
    private IImpactEffect[] impactArray;

    /// <summary>
    /// 移动模式
    /// </summary>
    private IMoveMode imoveMode;

    private bool interrupt = false;
    private IInterruptCondition[] ic;

    /// <summary>
    /// 初始化释放器
    /// </summary>
    private void InitDeployer()
    {
        //影响
        impactArray = DeployerConfigFactory.CreateImpactEffects(SkillData);

        //运动
        imoveMode = DeployerConfigFactory.CreateMoveMode(skillData);
    }

    /// <summary>
    /// 遍历影响列表并执行
    /// </summary>
    public void ImpactTargets()
    {
        for (int i = 0; i < impactArray.Length; i++)
        {
            impactArray[i].Execute(this);
        }
    }

    /// <summary>
    /// 执行对应的移动模式
    /// </summary>
    public void MoveMode()
    {
        imoveMode.Excute(this);
    }
    
    /// <summary>
    /// 检测自我终止条件
    /// </summary>
    public void CheckSelfInterruption()
    {
        if (SkillData.canSelfInterrupt)
        {
            ic = DeployerConfigFactory.CheckInterruptCondition(SkillData);
            StartCoroutine(CheckIC());
        }
    }

    /// <summary>
    /// 自我监测条件判断以及终止后的执行方法
    /// </summary>
    /// <returns></returns>
    private bool CheckInterruptCondition()
    {
        foreach (IInterruptCondition temp in ic)
        {
            if (temp.Check(this))
            {
                interrupt = true;
            }
        }
        if (interrupt)
        {
            foreach (IInterruptCondition ti in ic)
            {
                ti.OnFinish(this);
            }
        }
        return interrupt;
    }

    /// <summary>
    /// 自我终止监测协程
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckIC()
    {
        WaitForSeconds time = new WaitForSeconds(0.1f);
        while (!CheckInterruptCondition())
        {
            yield return time;
        }
        if (SkillData.hasAnimAfterInterrupt)
        {
            SkillData.owner.GetComponent<Animator>().SetBool(SkillData.animationName, false);
            transform.GetComponent<Animator>().SetBool(SkillData.animNameAfterInterrupt, true);
        }
        else
            this.Destroy();
    }

    /// <summary>
    /// 释放方式（供技能系统调用，由子类释放，定义具体释放策略）
    /// </summary>
    public abstract void DeploySkill();

    /// <summary>
    /// 销毁技能（供动画状态机调用）
    /// </summary>
    public virtual void Destroy()
    {
        PlayerStatus.Instance.InputEnable = true;
        PlayerStatus.Instance.EnableGravity = true;
        PlayerStatus.Instance.CanFlip = true;
        //从运行字典中移除
        SkillMgr.Instance.RemoveExcutingSkill(SkillData.skillID);
        if (skillData.audioSource != null && skillData.audioSource.loop)
            MusicMgr.Instance.StopSound(skillData.audioSource);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 技能冷却计算（供技能系统调用）
    /// </summary>
    public void SetCoolDown()
    {
        if (skillData.coolTime == 0) return;
        //因为只有冲刺和普攻是有减CD的
        float index = skillData.attackType == SkillAttackType.Sword ? PlayerStatus.Instance.AttackIntervalRate : PlayerStatus.Instance.SprintCDRate;
        skillData.coolDownTime = Time.time + skillData.coolTime * index;
        //把同为连招的所有技能同步释放CD
        SkillData temp = skillData;
        while (temp.isBatter)
        {
            SkillData another = ResMgr.Instance.Load<SkillData>(temp.nextBatterID);
            another.coolDownTime = skillData.coolDownTime;
            if (another.skillID == skillData.skillID) break;
            temp = another;
        }
    }
    public void SetComboTime()
    {
        skillData.loseComboTime = Time.time + skillData.comboMaxEffectTime;
        SkillData temp = skillData;
        while (temp.isBatter)
        {
            SkillData another = ResMgr.Instance.Load<SkillData>(temp.nextBatterID);
            another.loseComboTime = skillData.loseComboTime;
            if (another.skillID == skillData.skillID) break;
            temp = another;
        }
    }
}
