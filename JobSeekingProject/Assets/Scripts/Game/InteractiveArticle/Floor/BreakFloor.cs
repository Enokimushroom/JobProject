using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakFloor : TriggerBase
{
    private bool isTrigger;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Action()
    {
        if (!isTrigger)
        {
            isTrigger = true;
            anim.SetTrigger("Fall");
            MusicMgr.Instance.PlaySound("BreakFloorAudio", false);
        }
    }
    
    private void StartShake()
    {
        Debug.Log("shake");
    }

    public void StopInput()
    {
        GameManager.Instance.playerGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerStatus.Instance.InputEnable = false;
    }

    public void ResumeInput()
    {
        PlayerStatus.Instance.InputEnable = true;
    }
}
