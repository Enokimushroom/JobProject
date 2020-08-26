using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour,IDamagable
{
    protected bool isDeath = false;
    protected int health;

    protected Animator anim;

    public virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public virtual void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        ad.damageAmount = 1;
        health -= (int)ad.damageAmount;
        if (health <= 0)
        {
            isDeath = true;
            Dead();
            return;
        }
    }

    public virtual void Dead()
    {
        anim.SetTrigger("Death");
    }
}
