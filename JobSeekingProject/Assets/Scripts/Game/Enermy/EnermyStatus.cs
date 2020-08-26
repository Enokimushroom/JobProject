using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public delegate void EnermyDeathListener(EnermyStatus es);

public class EnermyStatus : MonoBehaviour,IDamagable
{
    [SerializeField] private string enermyID;
    public string EnermyID { get { return enermyID; } set { enermyID = value; } }

    [SerializeField] private string enermyName;
    public string EnermyName { get { return enermyName; } set { enermyName = value; } }
    public GenerateType generateType;

    private bool isHurt;
    [SerializeField] private float unhurtTime;
    public event EnermyDeathListener OnDeathEvent;

    private void Update()
    {
        if (unhurtTime>0)
        {
            isHurt = true;
            unhurtTime -= Time.deltaTime;
        }
        else
        {
            if (isHurt)
                isHurt = false;
        }
    }

    public void Damage(AttackDetails ad)
    {
        if(!isHurt)
            GetComponentInParent<Entity>().Damage(ad);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AttackDetails ad = new AttackDetails();
            ad.damageAmount = 1;
            ad.position = transform.position;
            collision.gameObject.GetComponent<IDamagable>().Damage(ad);
        }
    }

    public void DeathEvent()
    {
        OnDeathEvent?.Invoke(this);
        //如果猎人日志里面已经存在此怪物并且个数已经大于等于所需解锁个数。则pass（判断写到数据管理器中了）
        GameDataMgr.Instance.AddHunterItem(int.Parse(EnermyID.Replace("En", string.Empty)));
    }
}
