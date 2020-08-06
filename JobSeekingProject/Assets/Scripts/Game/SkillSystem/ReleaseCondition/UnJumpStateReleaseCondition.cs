using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnJumpStateReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return PlayerStatus.Instance.OnGround;
    }
}
