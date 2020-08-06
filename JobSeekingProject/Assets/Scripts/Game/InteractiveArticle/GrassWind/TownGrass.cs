using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGrass : MonoBehaviour, IDamagable
{
    [SerializeField] private string grassMoveAudioName;
    [SerializeField] private string grassCutAudioName;
    private int health = 1;
    private bool isDeath = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enermy"))
        {
            MusicMgr.Instance.PlaySound(grassMoveAudioName, false);
        }
    }

    public void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        ad.damageAmount = 1;
        health -= (int)ad.damageAmount;
        if (health <= 0)
        {
            isDeath = true;
            Dead();
        }
    }

    private void Dead()
    {
        PEManager.Instance.GetParticleEffectOneOff("TownGrassCut", this.transform, Vector3.zero, Vector3.one, Quaternion.identity);
        MusicMgr.Instance.PlaySound(grassCutAudioName, false);
        anim.SetTrigger("Death");
    }
}
