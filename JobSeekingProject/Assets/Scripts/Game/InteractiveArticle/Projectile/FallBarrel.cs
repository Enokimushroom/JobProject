using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBarrel : Projectile
{
    [SerializeField] private float rotateSpeed;
    private bool selfRot;

    private void FixedUpdate()
    {
        if (selfRot)
        {
            SelfRotate();
        }
    }

    private void SelfRotate()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.fixedDeltaTime));
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            selfRot = false;
            transform.rotation = Quaternion.identity;
            attackDetails.position = transform.position;
            attackDetails.damageAmount = 1;
            collision.transform.GetComponent<IDamagable>().Damage(attackDetails);
            Destroy();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            selfRot = false;
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
            anim.SetBool("OnGround", true);
        }
        else if (collision.gameObject.CompareTag("Enermy"))
        {
            selfRot = false;
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
            Destroy();
        }
    }

    public override void MoveType()
    {
        selfRot = true;
    }
}
