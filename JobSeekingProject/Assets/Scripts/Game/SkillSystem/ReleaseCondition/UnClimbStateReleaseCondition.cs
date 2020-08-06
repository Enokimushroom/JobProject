using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnClimbStateReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return !PlayerStatus.Instance.IsWallSliding;
    }
}
