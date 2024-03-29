﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGrass : Breakable
{
    [SerializeField] private string grassCutAudioName;
    [SerializeField] private int hp;

    public override void Start()
    {
        base.Start();
        health = hp;
    }

    public override void Dead()
    {
        PEManager.Instance.GetParticleEffectOneOff("TownGrassCut", this.transform, Vector3.zero, Vector3.one, Quaternion.identity);
        MusicMgr.Instance.PlaySound(grassCutAudioName, false);

        base.Dead();
    }
}
