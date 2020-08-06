using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbStateReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return PlayerStatus.Instance.IsWallSliding;
    }
}
