using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailChampionSlash : Projectile
{
    public override void MoveType()
    {
        rb.velocity = new Vector2(speed * facingDirection, 0);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            attackDetails.position = transform.position;
            collision.transform.GetComponent<IDamagable>().Damage(attackDetails);
            Destroy();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector2.zero;
            Destroy();
        }
    }

}
