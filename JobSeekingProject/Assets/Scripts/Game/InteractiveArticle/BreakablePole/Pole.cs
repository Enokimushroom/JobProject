using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : Breakable
{

    public override void Start()
    {
        health = 1;
    }

    public override void Dead()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetComponent<SpriteRenderer>().enabled = false;
        CinemachineShake.Instance.ShakeCamera(1.0f, 0.5f);
        MusicMgr.Instance.PlaySound("PoleBreakDeath", false);
    }
}
