using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostSPReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return PlayerStatus.Instance.SP >= data.costSP;
    }

}
