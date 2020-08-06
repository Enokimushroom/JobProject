using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDetails
{
    //攻击给予方的位置
    public Vector2 position;
    //攻击伤害
    public float damageAmount;
    //伤害种类
    public SkillAttackType type;
    //眩晕阶段伤害所需
    public int stunDamageAmount;
}
