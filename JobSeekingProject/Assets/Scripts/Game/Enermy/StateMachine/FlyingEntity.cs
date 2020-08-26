using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEntity : Entity
{
    protected float flydamageHop = 1;

    public override bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.up, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public override bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.OverlapCircle(playerCheck.position, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public override bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.OverlapCircle(transform.position, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, entityData.groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.up * entityData.ledgeCheckDistance));
        
        Gizmos.DrawWireSphere(playerCheck.position, entityData.minAgroDistance);
        Gizmos.DrawWireSphere(transform.position, entityData.maxAgroDistance);
    }
}
