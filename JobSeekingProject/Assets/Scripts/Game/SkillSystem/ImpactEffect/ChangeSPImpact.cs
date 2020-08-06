using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消耗SP
/// </summary>
public class ChangeSPImpact : IImpactEffect
{
    private bool costSpDone;

    public void Execute(Deployer deployer)
    {
        if (!costSpDone)
        {

            PlayerStatus.Instance.ChangeSP(-1 * deployer.SkillData.costSP * PlayerStatus.Instance.MagicCostRate);
            costSpDone = true;
        }
    }
}
