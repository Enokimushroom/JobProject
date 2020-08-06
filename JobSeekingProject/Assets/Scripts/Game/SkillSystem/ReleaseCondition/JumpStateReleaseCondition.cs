using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateReleaseCondition : IReleaseCondition
{
    public bool CheckCondition(SkillData data)
    {
        return !PlayerStatus.Instance.OnGround;
    }
}
