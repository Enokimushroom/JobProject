using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HealingImpact : IImpactEffect
{
    public void Execute(Deployer deployer)
    {
        PlayerStatus.Instance.ChangeCurrentHealth(1 * 1 * (int)PlayerStatus.Instance.HealingAmountRate);
    }
}
