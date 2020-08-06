using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 封装技能系统，对外提供释放功能
/// </summary>
public class CharacterSkillSystem : MonoBehaviour
{
    private Animator anim;
    private SkillData skill;
    private string tempID;

    #region 蓄力
    private CustomButton chargeButton;
    private float chargeTime;
    private bool chargeStop;
    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();
        GetComponent<AnimatorEventBehaviour>().attackHandler += DeploySkill;
    }

    private void FixedUpdate()
    {
        CheckPassiveSkill();
    }

    /// <summary>
    /// 释放技能（代码委托给技能动画OnAttack委托）
    /// </summary>
    private void DeploySkill()
    {
        //生成技能并存入字典
        SkillMgr.Instance.excutingSkill[tempID] = SkillMgr.Instance.GenerateSkill(skill);
    }

    /// <summary>
    /// 使用技能攻击（响应玩家输入使用技能/自检使用被动技能）
    /// </summary>
    public void AttackUstSkill(SkillData skillData, CustomButton cb = null)
    {
        skill = SkillMgr.Instance.CheckBatter(skillData,cb);
        if (!SkillCheckBeforeRelease(skill)) return;
        SkillMgr.Instance.excutingSkill.Add(skill.skillID, null);
        //识别释放模式（是否蓄力技能）
        if (skill.isCharge)
        {
            chargeButton = cb;
            chargeTime = 0;
            StartCoroutine(Charging(skill.skillID));
            return;
        }
        //播放动画
        anim.SetBool(skill.animationName, true);
        tempID = skill.skillID;
    }

    /// <summary>
    /// 判断被动技能的释放
    /// </summary>
    private void CheckPassiveSkill()
    {
        foreach(SkillData sk in SkillMgr.Instance.badgeSkill.Values)
        {
            if (sk.passiveSkill)
            {
                if (!SkillCheckBeforeRelease(sk)) return;
                SkillMgr.Instance.excutingSkill.Add(sk.skillID, null);
                if (skill.animationName != string.Empty)
                    anim.SetBool(skill.animationName, true);
                else
                    SkillMgr.Instance.GeneratePassiveSkill(sk);
            }
        }
    }
    
    /// <summary>
    /// 是否满足释放前置条件
    /// </summary>
    private bool SkillCheckBeforeRelease(SkillData data)
    {
        //如果还没拥有这个技能，返回
        if (!SkillMgr.Instance.FixSkill.ContainsKey(data.skillID) && !SkillMgr.Instance.badgeSkill.ContainsKey(data.skillID))
        {
            Debug.Log("玩家还没这个技能");
            return false;
        }
        //如果已经在执行中，返回
        if (SkillMgr.Instance.excutingSkill.ContainsKey(data.skillID))
        {
            Debug.Log("技能正在执行");
            return false;
        }
        //优先判断技能释放条件，如果不满足条件，return
        if (!SkillMgr.Instance.CheckSkillCondition(data))
        {
            Debug.Log("技能条件未满足");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 蓄力协程
    /// </summary>
    /// <returns></returns>
    IEnumerator Charging(string id)
    {
        EventCenter.Instance.AddEventListener<KeyCode>("xUp", CheckKeyUp);
        EventCenter.Instance.AddEventListener<KeyCode>("xPressing", CheckKeyPressing);
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.EnableGravity = false;
        bool interupt = false;
        chargeStop = false;
        GameObject po = null;
        anim.SetBool(skill.chargeAnimName, true);
        //TODO:超级冲刺蓄力音效开启
        //关闭角色重力
        PlayerStatus.Instance.EnableGravity = false;
        //蓄力特效物体开启
        PEManager.Instance.GetParticleObject(skill.chargePEName, skill.owner.transform, Vector3.zero);
        //只有治疗的蓄力时间会受到护符影响
        float index = skill.skillID == "S004" ? PlayerStatus.Instance.HealingSpeedRate : 1;
        float cmpChargeTime = skill.chargeTime * index;
        while (!chargeStop)
        {
            if (PlayerStatus.Instance.IsHit)
            {
                interupt = true;
                break;
            }
            if (skill.executeOnceChargeCompleted && chargeTime >= cmpChargeTime)
            {
                Debug.Log("蓄力已完成，强制释放");
                break;
            }
            chargeTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        if (chargeTime >= cmpChargeTime && !interupt)
        {
            Debug.Log("蓄力技能释放成功");
            anim.SetBool(skill.animationName, true);
            tempID = id;
        }
        else if (chargeTime < cmpChargeTime || interupt)
        {
            Debug.Log("蓄力技能释放失败");
            //重新开启角色重力
            PlayerStatus.Instance.EnableGravity = true;
            SkillMgr.Instance.RemoveExcutingSkill(skill.skillID);
        }
        anim.SetBool(skill.chargeAnimName, false);
        //TODO:超级冲刺蓄力音效关闭
        //蓄力特效物体关闭
        PEManager.Instance.BackParticleObject(skill.chargePEName);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPressing", CheckKeyPressing);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xUp", CheckKeyUp);
    }

    #region 监听蓄力输入与终止
    private void CheckKeyPressing(KeyCode key)
    {
        if (chargeButton != null)
        {
            if (key == chargeButton.CurrentKey)
            {
                chargeStop = false;
            }

        }
    }
    private void CheckKeyUp(KeyCode key)
    {
        if (key == chargeButton.CurrentKey)
        {
            chargeStop = true;
        }
    }
    #endregion
}
