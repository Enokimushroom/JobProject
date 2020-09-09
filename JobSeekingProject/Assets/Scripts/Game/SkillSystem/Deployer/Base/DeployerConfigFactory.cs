using System;
using UnityEngine;

/// <summary>
/// 释放器配置工厂:提供创建释放器各种算法对象的功能
/// </summary>
public class DeployerConfigFactory
{
    public static IReleaseCondition[] CreateReleaseCondition(SkillData data)
    {
        IReleaseCondition[] conditions = new IReleaseCondition[data.releaseCondition.Length];
        //命名规范:releaseCondition[?] + ReleaseCondition
        for(int i = 0; i < data.releaseCondition.Length; ++i)
        {
            string className = string.Format("{0}ReleaseCondition", data.releaseCondition[i]);
            conditions[i] = CreatObject<IReleaseCondition>(className);
        }
        return conditions;
    }

    public static IImpactEffect[] CreateImpactEffects(SkillData data)
    {
        IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
        //命名规范：impactTpye[?] + Impact
        //例如消耗法力：CostSPImpact
        for (int i = 0; i < data.impactType.Length; ++i)
        {
            string classNameImpact = string.Format("{0}Impact", data.impactType[i]);
            impacts[i] = CreatObject<IImpactEffect>(classNameImpact);
        }
        return impacts;
    }

    public static IMoveMode CreateMoveMode(SkillData data)
    {
        //移动模式
        //命名规范：类似LongRangeMode
        string className = string.Format("{0}Mode", data.moveType);
        return CreatObject<IMoveMode>(className);
    }

    public static IInterruptCondition[] CheckInterruptCondition(SkillData data)
    {
        IInterruptCondition[] ic = new IInterruptCondition[data.selfInterruptCondition.Length];
        for(int i = 0; i < data.selfInterruptCondition.Length; ++i)
        {
            string className = string.Format("{0}IC", data.selfInterruptCondition[i]);
            ic[i] = CreatObject<IInterruptCondition>(className);
        }
        return ic;
    }

    private static T CreatObject<T>(string className) where T : class
    {
        Type type = Type.GetType(className);
        return Activator.CreateInstance(type) as T;
    }
}
