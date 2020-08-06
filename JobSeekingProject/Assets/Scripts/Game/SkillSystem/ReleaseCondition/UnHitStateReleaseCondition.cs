using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnHitStateReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return !PlayerStatus.Instance.IsHit;
    }
}
