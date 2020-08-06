using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfollowReleaseMode : IMoveMode
{
    public void Excute(Deployer deployer)
    {
        int face = PlayerStatus.Instance.IsFacingRight ? 1 : -1;
        float moveDis = deployer.SkillData.skillSpeed * deployer.SkillData.duration * face;
        deployer.transform.DOMoveX(deployer.SkillData.owner.transform.position.x + moveDis, deployer.SkillData.duration).onComplete = () => 
        {
            deployer.Destroy();
        };
    }
}
