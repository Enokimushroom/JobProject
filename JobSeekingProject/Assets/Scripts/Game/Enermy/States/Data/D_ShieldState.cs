using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newShieldStateData",menuName ="Data/State Data/ShieldState Data")]
public class D_ShieldState : ScriptableObject
{
    public float offsetY;
    public float checkX;
    public float checkY;
    public float shieldTime;
    public LayerMask whatIsPlayer;
}
