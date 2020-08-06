using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReleaseCondition
{
    bool CheckCondition(SkillData data);
}
