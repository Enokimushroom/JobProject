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

    public float HP;
    public bool isHurt;
    public float unhurtTime;
    public event EnermyDeathListener OnDeathEvent;

    private void Update()
    {
        if (unhurtTime>0)
        {
            unhurtTime -= Time.deltaTime;
        }
        else
        {
            isHurt = false;
        }
    }

    public void Damage(AttackDetails ad)
    {
        GetComponentInParent<Entity>().Damage(ad);
        //TODO: 击中特效和音效
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && PlayerStatus.Instance.CanBeHurt)
        {
            Debug.Log("trigger");
            PlayerStatus.Instance.CanBeHurt = false;
            AttackDetails ad = new AttackDetails();
            ad.damageAmount = 1;
            ad.position = transform.position;
            collision.gameObject.GetComponent<IDamagable>().Damage(ad);
        }
    }

    public void OnDestroy()
    {
        OnDeathEvent?.Invoke(this);
    }
}
