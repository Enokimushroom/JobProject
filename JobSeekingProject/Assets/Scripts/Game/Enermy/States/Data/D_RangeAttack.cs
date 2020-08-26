using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName ="newRangeAttackStateData",menuName ="Data/State Data/RangeAttack State")]
public class D_RangeAttack : ScriptableObject
{
    public string projectileName;
    public float projectileDamage = 1;
    public float projectileSpeed = 12.0f;
    public float projectileHeight;
    public float attackCD;
}
