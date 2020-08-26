using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="new Skill",menuName ="Skill/new SkillData")]
public class SkillData : ScriptableObject
{
    [Tooltip("用作数据查找的技能ID")]
    public string skillID;
    [Tooltip("没用的，就为了在Inspector上看名字方便")]
    public string skillName;
    #region 按键
    [Tooltip("技能按键")]
    public ReflectKey key;
    [Tooltip("专门给普攻的生成位置偏移")]
    [ConditionalHide("key", (int)ReflectKey.attack, true)] public float offsetX;
    #endregion

    #region 技能类型选择
    [Header("技能类型选择")]
    [Tooltip("固定技能还是护符技能")]//固定技能都是要按键输入，护符技能都是被动检测
    public SkillTag skillTag;
    [ConditionalHide("skillTag", (int)SkillTag.BadgeSkill, true)] public bool passiveSkill;
    [Tooltip("剑术还是魔法")]
    public SkillAttackType attackType;
    [Tooltip("远程还是近程")]
    public SkillMoveMode moveType;
    [Tooltip("移动速度")]
    [ConditionalHide("moveType",(int)SkillMoveMode.UnfollowRelease,true)] public float skillSpeed;
    #endregion

    #region 冷却与SP
    [Header("冷却与SP")]
    [Tooltip("耗费SP")]
    public float costSP;
    [Tooltip("是否有冷却时间")]
    public bool hasCoolTime;
    [ConditionalHide("hasCoolTime", true)]
    [Tooltip("冷却时间")]
    public float coolTime;
    [Tooltip("开始冷却时间")]
    [HideInInspector] public float coolDownTime;
    #endregion

    #region 蓄力
    [Header("蓄力")]
    [Tooltip("是否是蓄力技能")]
    public bool isCharge;
    [Tooltip("蓄力时间")]
    [ConditionalHide("isCharge", true, false)] public float chargeTime;
    [Tooltip("蓄力完结后是否立即执行")]
    [ConditionalHide("isCharge", true, false)] public bool executeOnceChargeCompleted;
    [Tooltip("蓄力动画")]
    [ConditionalHide("isCharge", true, false)] public string chargeAnimName;
    [Tooltip("蓄力预制体名字")]
    [ConditionalHide("isCharge", true, false)] public string chargePEName;
    [Tooltip("蓄力音效名称")]
    [ConditionalHide("isCharge", true, false)] public string chargeAudioName;
    [Tooltip("蓄力成功音效名称")]
    [ConditionalHide("isCharge", true, false)] public string chargeSucceedAudioName;
    #endregion

    #region 连击
    [Header("连击")]
    [Tooltip("是否连击技能")]
    public bool isBatter;
    [Tooltip("下个连击技能ID")]
    [ConditionalHide("isBatter", true, false)] public string nextBatterID;
    [ConditionalHide("isBatter", true, false)] public float comboMaxEffectTime;
    [HideInInspector] public float loseComboTime;

    #endregion

    #region 伤害
    [Header("伤害")]
    [Tooltip("是否会造成伤害")]
    public bool hasDamage;
    [Tooltip("技能伤害")]//伤害公式 = (baseATK+baseDamage)*法术系数or剑术系数
    [ConditionalHide("hasDamage", true, false)] public float baseDamage;
    [Tooltip("伤害间隔")]
    [ConditionalHide("hasDamage", true, false)] public float damageInterval;
    [Tooltip("伤害计算持续时间（某些情况下也用来当作技能持续时间，比如发射物")]
    [ConditionalHide("hasDamage", true, false)] public float duration;
    #endregion

    #region 释放以及击中后坐力
    [Header("击中后坐力")]
    [Tooltip("是否有击中后坐力")]
    public bool hasRecoil;
    [Tooltip("击中后坐力速度")]
    [ConditionalHide("hasRecoil", true, false)] public float hitRecoilSpeed;
    [Tooltip("击中后坐力时间")]
    [ConditionalHide("hasRecoil", true, false)] public float hitRecoilTime;
    [Header("释放后坐力")]
    [Tooltip("是否有释放后坐力")]
    public bool hasReleaseRecoil;
    [Tooltip("释放后坐力方向")]
    [ConditionalHide("hasReleaseRecoil", true, false)] public RecoilDirection releaseRecoilDirection;
    [Tooltip("释放后坐力速度")]
    [ConditionalHide("hasReleaseRecoil", true, false)] public float releaseRecoilSpeed;
    [Tooltip("释放后坐力时间")]
    [ConditionalHide("hasReleaseRecoil", true, false)] public float releaseRecoilTime;
    #endregion

    #region 属性
    [Header("属性")]
    [Tooltip("是否更改属性")]
    public bool changeAtr;
    [Tooltip("更改人物属性种类")]
    [ConditionalHide("changeAtr", true, false)] public PlayerInfoType info;
    [Tooltip("更改人物属性数值")]
    [ConditionalHide("changeAtr", true, false)] public float delta;
    #endregion

    #region 释放效果与释放条件
    [Header("释放效果与释放条件")]
    [Tooltip("对人物的影响效果")]
    public SkillImpactType[] impactType;
    [Tooltip("释放条件")]
    public SkillReleaseConditionType[] releaseCondition;
    #endregion

    #region 其他
    [Header("重要信息")]
    [Tooltip("技能发出者")]
    public GameObject owner;
    [Tooltip("技能预制体名字")]
    public string prefabName;
    [Tooltip("人物动画名字")]
    public string animationName;
    [Tooltip("音效名称")]
    public string audioName;
    [HideInInspector] public AudioSource audioSource;
    public bool audioLoop;
    #endregion

    #region 中断
    [Header("中断")]
    [Tooltip("是否可被中断")] 
    public bool canSelfInterrupt;
    [Tooltip("中断后是否有结束动画")]
    [ConditionalHide("canSelfInterrupt", true, false)] public bool hasAnimAfterInterrupt;
    [Tooltip("中断后结束动画的名字")]
    [ConditionalHide("hasAnimAfterInterrupt", true, false)] public string animNameAfterInterrupt;
    [Tooltip("被中断的条件")]
    public SelfInterruptedCondition[] selfInterruptCondition;
    #endregion

    #region 组合按键
    [Header("组合按键")]
    [Tooltip("是否存在组合按键")]
    public bool hasComboKey;
    [Tooltip("组合按键指向的技能ID")]
    [ConditionalHide("hasComboKey", true, false)] public string upKeySkillID;
    [ConditionalHide("hasComboKey", true, false)] public string downKeySkillID;
    #endregion

    #region 按键时长区别
    [Header("按键时长区别")]
    [Tooltip("是否有按键时长区别")]
    public bool differentByPressingTime;
    [ConditionalHide("differentByPressingTime", true, false)] public string shortPressingSkillID;
    #endregion

    #region 是否可被护符技能覆盖
    [Header("是否可被护符技能覆盖")]
    public bool canBeChangeByBadge;
    [ConditionalHide("canBeChangeByBadge", true, false)] public string skillIDIfChanged;
    #endregion
}

public enum ReflectKey
{
    attack=1,
    jump=2,
    sprint=3,
    superSprint=4,
    recover=5,
    none=6,
}

public enum RecoilDirection
{
    Up=1,
    Down=2,
    Left=3,
    Right=4,
}

public enum SelfInterruptedCondition
{
    CheckKey=1,
    CheckWall=2,
    CheckGround=3,
    CheckOn=4,
    CheckDying=5,
}

