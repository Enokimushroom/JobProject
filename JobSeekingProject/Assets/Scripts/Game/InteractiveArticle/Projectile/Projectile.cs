using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected AttackDetails attackDetails;
    protected float speed;
    protected float travelDistance;
    protected float facingDirection;

    protected Rigidbody2D rb;
    protected Animator anim;

    [SerializeField] private string projectileName;
    [SerializeField] private float damageRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform damagePosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public virtual void SetProjectile(string name,float speed, float travelDistance, float damage,float facingDirection)
    {
        this.projectileName = name;
        this.speed = speed;
        this.facingDirection = facingDirection;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            attackDetails.position = transform.position;
            collision.transform.GetComponent<IDamagable>().Damage(attackDetails);
            Debug.Log(attackDetails.damageAmount);
            Destroy();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("OnGround", true);
        }
    }

    public abstract void MoveType();

    public void Destroy()
    {
        PoolMgr.Instance.BackObj(projectileName, gameObject);
    }

}
