using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour,IDamagable
{
    private int health = 1;
    private bool canHurt = true;
    private bool isDeath = false;

    public void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        ad.damageAmount = 1;
        health -= (int)ad.damageAmount;
        CinemachineShake.Instance.ShakeCamera(1.0f, 0.5f);
        PEManager.Instance.GetParticleEffectOneOff("LittleRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        if (health <= 0)
        {
            isDeath = true;
            MusicMgr.Instance.PlaySound("StalactiteBreak", false);
            transform.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canHurt = false;
            PEManager.Instance.BackParticleEffect("StalactiteDustTrailPE");
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            MusicMgr.Instance.PlaySound("StalactiteImpact", false);
        }
        if (collision.CompareTag("Player") && canHurt)
        {
            canHurt = false;
            AttackDetails ad = new AttackDetails();
            ad.damageAmount = 1;
            ad.position = transform.position;
            collision.GetComponent<IDamagable>().Damage(ad);
        }

    }
}
