using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentHealth : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return PlayerStatus.Instance.CurrentHealth < PlayerStatus.Instance.MaxHealth;
    }
}
