using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobProjectile : Projectile
{
    public override void MoveType()
    {
        Vector2 force = AddForceCalculate.CalculateFroce(transform.position, GameObject.FindWithTag("Player").transform.position, travelDistance);
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
