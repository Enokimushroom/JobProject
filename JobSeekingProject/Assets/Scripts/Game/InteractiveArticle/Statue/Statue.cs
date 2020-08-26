using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : Breakable
{
    [SerializeField] private string breakAudioName;

    public override void Start()
    {
        base.Start();

        health = 1;
    }

    public override void Dead()
    {
        PEManager.Instance.GetParticleEffectOneOff("LittleRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        MusicMgr.Instance.PlaySound(breakAudioName, false);

        base.Dead();
    }

}
