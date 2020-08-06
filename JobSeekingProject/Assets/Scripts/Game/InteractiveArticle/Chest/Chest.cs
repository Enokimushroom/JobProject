using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IDamagable
{
    [SerializeField]private int itemID;
    [SerializeField] private int itemNum;
    private bool isDeath;
    private int respawnDirection;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Damage(AttackDetails ad)
    {
        if (isDeath) return;
        isDeath = true;
        Vector2 v = ad.position - new Vector2(transform.position.x, transform.position.y) ;
        respawnDirection = v.x > 0 ? -1 : 1;
        //TODO: 产生刀光
        anim.SetTrigger("Open");
        PEManager.Instance.GetParticleEffectOneOff("ChestOpenRing", transform, Vector3.zero, Vector3.one, Quaternion.Euler(80, 0, 0));

    }

    public void RespawnItem()
    {
        GameObject item = ResMgr.Instance.Load<GameObject>("ChestItem");
        item.transform.position = transform.position;
        Vector2 force = new Vector2(Random.Range(80, 150) * respawnDirection, 800);
        item.GetComponentInChildren<Rigidbody2D>().AddForce(force, ForceMode2D.Force);
        item.GetComponent<ChestItem>().SetItemID(itemID, itemNum);
    }
}
