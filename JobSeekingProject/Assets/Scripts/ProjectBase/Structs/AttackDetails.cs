using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDetails
{
    //攻击给予方的位置
    public Vector2 position;
    //攻击伤害
    public float damageAmount;
    //攻击类型
    public SkillAttackType type;
}
