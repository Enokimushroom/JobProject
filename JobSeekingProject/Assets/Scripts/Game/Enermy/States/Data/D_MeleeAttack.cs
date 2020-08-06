using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/MeleeAttackState Data")]
public class D_MeleeAttack : ScriptableObject
{
    public float attackRadius = 0.5f;
    public int attackDamage = 10;

    public LayerMask whatIsPlayer;
}
