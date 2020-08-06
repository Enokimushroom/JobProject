using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloor : MonoBehaviour,IDamagable
{
    [SerializeField] private int health;
    private bool isDeath;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        ad.damageAmount = 1;
        health -= (int)ad.damageAmount;
        if (health <= 0)
        {
            isDeath = true;
            Death();
            return;
        }
        PEManager.Instance.GetParticleEffectOneOff("FloorDustHurtPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(90,90,-90));
        PEManager.Instance.GetParticleEffectOneOff("FloorWoodHurtPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, -180));
        PEManager.Instance.GetParticleEffectOneOff("FloorStoneHurtPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, 180));
        anim.SetTrigger("Hurt");
    }

    private void Death()
    {
        PEManager.Instance.GetParticleEffectOneOff("FloorDustBreakPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(90, 90, -90));
        PEManager.Instance.GetParticleEffectOneOff("FloorWoodBreakPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, -180));
        PEManager.Instance.GetParticleEffectOneOff("FloorStoneBreakPE", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        anim.SetTrigger("Death");
    }



}
