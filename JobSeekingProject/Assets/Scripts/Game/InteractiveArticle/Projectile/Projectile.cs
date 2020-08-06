using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails attackDetails;
    private float speed;
    private float travelDistance;

    private bool hasHitGround;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private string projectileName;
    [SerializeField] private float damageRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform damagePosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hasHitGround = false;
    }

    public void SetProjectile(string name,float speed, float travelDistance, float damage)
    {
        this.projectileName = name;
        this.speed = speed;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }

    private void Update()
    {
        if (!hasHitGround)
        {
            attackDetails.position = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.GetComponent<IDamagable>().Damage(attackDetails);
            Destroy();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasHitGround = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("OnGround", true);
        }
    }

    

    private void Destroy()
    {
        PoolMgr.Instance.BackObj(projectileName, gameObject);
    }

}
