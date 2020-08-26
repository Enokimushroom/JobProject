using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Breakable
{
    [SerializeField]private int itemID;
    [SerializeField] private int itemNum;
    [SerializeField] private int hp;
    private int respawnDirection;

    public override void Start()
    {
        base.Start();

        health = hp;
    }

    public override void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        isDeath = true;
        Vector2 v = ad.position - new Vector2(transform.position.x, transform.position.y) ;
        respawnDirection = v.x > 0 ? -1 : 1;
        anim.SetTrigger("Open");
        PEManager.Instance.GetParticleEffectOneOff("ChestOpenRing", transform, Vector3.zero, Vector3.one, Quaternion.Euler(80, 0, 0));
    }

    private void RespawnItem()
    {
        GameObject item = ResMgr.Instance.Load<GameObject>("ChestItem");
        item.transform.position = transform.position;
        Vector2 force = new Vector2(Random.Range(80, 150) * respawnDirection, 800);
        item.GetComponentInChildren<Rigidbody2D>().AddForce(force, ForceMode2D.Force);
        item.GetComponent<ChestItem>().SetItemID(itemID, itemNum);
    }
}
