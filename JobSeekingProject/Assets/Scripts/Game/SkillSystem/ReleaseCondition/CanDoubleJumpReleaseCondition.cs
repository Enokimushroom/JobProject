using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDoubleJumpReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return PlayerStatus.Instance.CanDoubleJump;
    }
}
