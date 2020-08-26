using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCollisionTrigger : TriggerBase
{
    private Animator anim;
    [SerializeField] private string grassMoveAudioName;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public override void Action()
    {

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enermy"))
        {
            Vector3 dis = (collision.transform.position - transform.position).normalized;
            if (dis.x > 0)
                anim.SetTrigger("FromRight");
            else
                anim.SetTrigger("FromLeft");
            MusicMgr.Instance.PlaySound(grassMoveAudioName, false);
        }
    }
}
