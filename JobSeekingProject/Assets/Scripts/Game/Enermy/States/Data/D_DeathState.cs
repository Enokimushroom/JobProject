using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeathStateData", menuName = "Data/State Data/DeathState Data")]
public class D_DeathState : ScriptableObject
{
    public float deathSpeed = 15;
    public Vector2 deathDirection = new Vector2(1, 1);
    public float deathTime = 0.5f;
    public float deathStopTime = 5f;
    public float deathDissolveTime = 10f;
}
