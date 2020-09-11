using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomAttack : TriggerBase
{
    private bool attack;

    private void OnEnable()
    {
        attack = false;
    }

    public override void Action()
    {
        if (attack) return;
        attack = true;
        AttackDetails ad = new AttackDetails
        {
            damageAmount = 2,
            position = transform.position
        };
        collision.GetComponent<IDamagable>().Damage(ad);
    }
}
