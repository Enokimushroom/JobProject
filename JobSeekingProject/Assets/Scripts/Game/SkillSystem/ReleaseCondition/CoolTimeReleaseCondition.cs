using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeReleaseCondition : IReleaseCondition
{

    public bool CheckCondition(SkillData data)
    {
        if(Time.time >= data.coolDownTime)
        {
            return true;
        }
        else
        {
            Debug.Log("CD未满足");
            return false;
        }
    }
}
