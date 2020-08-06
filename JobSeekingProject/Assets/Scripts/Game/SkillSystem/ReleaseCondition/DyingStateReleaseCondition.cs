using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingStateReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return PlayerStatus.Instance.CurrentHealth <= 2;
    }
}
