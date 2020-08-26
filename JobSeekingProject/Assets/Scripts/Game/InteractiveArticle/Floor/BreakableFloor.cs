using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloor : Breakable
{
    [SerializeField] private int hp;

    public override  void Start()
    {
        base.Start();

        health = hp;
    }

    public override  void Damage(AttackDetails ad)
    {
        base.Damage(ad);

        PEManager.Instance.GetParticleEffectOneOff("FloorDustHurtPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(90,90,-90));
        PEManager.Instance.GetParticleEffectOneOff("FloorWoodHurtPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, -180));
        PEManager.Instance.GetParticleEffectOneOff("FloorStoneHurtPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, 180));
        anim.SetTrigger("Hurt");
    }

    public override void Dead()
    {
        PEManager.Instance.GetParticleEffectOneOff("FloorDustBreakPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(90, 90, -90));
        PEManager.Instance.GetParticleEffectOneOff("FloorWoodBreakPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, -180));
        PEManager.Instance.GetParticleEffectOneOff("FloorStoneBreakPE", transform, Vector3.zero, Vector3.one, Quaternion.identity);

        base.Dead();
    }



}
