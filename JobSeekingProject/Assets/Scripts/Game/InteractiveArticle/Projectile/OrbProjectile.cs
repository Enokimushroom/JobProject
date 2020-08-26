using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbProjectile : Projectile
{
    private float rotationAngle;
    private bool turnOn;
    private bool hit;
    private float startTime;
    private Vector3 targetPosition;

    private void Update()
    {
        if (turnOn && !hit)
        {
            if(Time.time <= startTime + travelDistance)
            {
                targetPosition = GameObject.FindWithTag("Player").transform.position;
                Vector3 direction = targetPosition - transform.position;
                float angle = 360 - Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
                transform.rotation = transform.rotation * Quaternion.Euler(0, 0, rotationAngle);
            }
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hit = true;
            rb.velocity = Vector2.zero;
            attackDetails.position = transform.position;
            collision.transform.GetComponent<IDamagable>().Damage(attackDetails);
            anim.SetBool("Hit", true);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hit = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("OnGround", true);
        }
    }

    public override void MoveType()
    {
        rotationAngle = Random.Range(-60, 10);
        transform.localScale = Vector3.one;
        turnOn = false;
        hit = false;
        startTime = Time.time;
    }

    private void TurnOn()
    {
        turnOn = true;
    }
}
