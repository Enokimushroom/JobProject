using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeRockDoor : Breakable
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
        anim.SetInteger("Health", health);
        Debug.Log(health);
        if (health <= 0)
        {
            isDeath = true;
            Dead();
            return;
        }
        PEManager.Instance.GetParticleEffectOneOff("LittleRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.Euler(new Vector3(0, 0, 90)));
    }

    public override void Dead()
    {
        transform.GetComponentInChildren<BoxCollider2D>().enabled = false;
        transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        PEManager.Instance.GetParticleEffectOneOff("MuchRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.Euler(new Vector3(0, 0, 90)));
    }

}
