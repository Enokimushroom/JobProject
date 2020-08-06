using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour,IDamagable
{
    [SerializeField] private string breakAudioName;
    private int health = 1;
    private bool isDeath = false;
    private Animator anim;

    private void Awake()
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
            Dead();
        }
    }

    private void Dead()
    {
        PEManager.Instance.GetParticleEffectOneOff("LittleRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        MusicMgr.Instance.PlaySound(breakAudioName, false);
        anim.SetTrigger("Death");
    }

}
