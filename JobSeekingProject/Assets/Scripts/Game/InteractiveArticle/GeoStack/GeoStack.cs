using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoStack : Breakable
{
    private int minCoinPerHit = 1;
    private int maxCoinPerHit = 4;
    private float maxBumpForceInX = 300;
    private float minBumpForceInY = 600;
    private float maxBumpForceInY = 800;
    [SerializeField] private int hp;

    public override void Start()
    {
        base.Start();
        health = hp;
    }

    private void SpawnCoins()
    {
        int randomCount = Random.Range(minCoinPerHit, maxCoinPerHit);
        for(int i = 0; i < randomCount; ++i)
        {
            GameObject geo = ResMgr.Instance.Load<GameObject>("Geo");
            geo.transform.position = transform.position;
            Vector2 force = new Vector2(Random.Range(-maxBumpForceInX, maxBumpForceInX), Random.Range(minBumpForceInY, maxBumpForceInY));
            geo.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Force);
        }
    }

    public override void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        ad.damageAmount = 1;
        health -= (int)ad.damageAmount;
        MusicMgr.Instance.PlaySound("GeoStackHit", false);
        if (health <= 0)
        {
            isDeath = true;
            Dead();
            return;
        }
        Vector2 v = ad.position - new Vector2(transform.position.x, transform.position.y);
        Vector3 localScale = v.x < 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        PEManager.Instance.GetParticleEffectOneOff("GeoDustPE", transform, Vector3.zero,localScale,Quaternion.identity);
        SpawnCoins();
        anim.SetTrigger("Hurt");
    }

    public override void Dead()
    {
        PEManager.Instance.GetParticleEffectOneOff("GeoDustPE", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        PEManager.Instance.GetParticleEffectOneOff("GeoDustPE", transform, Vector3.zero, new Vector3(-1, 1, 1), Quaternion.identity);
        PEManager.Instance.GetParticleEffectOneOff("GeoDustPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, 90));
        PEManager.Instance.GetParticleEffectOneOff("GeoStackRocksBurst", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, 90));

        base.Dead();
    }
}
