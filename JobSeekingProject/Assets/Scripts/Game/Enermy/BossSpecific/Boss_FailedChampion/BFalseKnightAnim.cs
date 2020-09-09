using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFalseKnightAnim : MonoBehaviour
{
    private void DeathBoomParticleOn()
    {
        PEManager.Instance.GetParticleEffect("EnermyDeadBoomEffect", transform, new Vector3(-1.5f, -1.5f, 0), Vector3.one, Quaternion.identity);
    }

    private void DeathBoomParticleOff()
    {
        PEManager.Instance.BackParticleEffect("EnermyDeadBoomEffect");
        PEManager.Instance.GetParticleEffectOneOff("HitEnermyBoomEffect", transform, new Vector3(-1.5f, -1.5f, 0), Vector3.one, Quaternion.identity);
    }


}
