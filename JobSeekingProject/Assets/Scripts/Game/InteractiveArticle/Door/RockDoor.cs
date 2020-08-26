
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDoor : Breakable
{
    [SerializeField] private int hp;

    public override void Start()
    {
        base.Start();

        health = hp;
    }

    public override void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        ad.damageAmount = 1;
        health -= (int)ad.damageAmount;
        MusicMgr.Instance.PlaySound("DoorBreak", false);
        PEManager.Instance.GetParticleEffectOneOff("DoorDustPE", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        if (health <= 0)
        {
            PEManager.Instance.GetParticleEffectOneOff("MuchRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.identity);
            isDeath = true;
            Dead();
            return;
        }
        PEManager.Instance.GetParticleEffectOneOff("LittleRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        anim.SetTrigger("Hurt");
    }

    public override void Dead()
    {
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("Death");
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

}
