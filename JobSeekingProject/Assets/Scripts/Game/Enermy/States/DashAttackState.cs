using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackState : AttackState
{
    protected D_DashAttack stateData;
    protected AttackDetails attackDetails;

    public DashAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_DashAttack stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        attackDetails.position = entity.aliveGO.transform.position;
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;
        entity.SetVelocity(stateData.dashSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

        foreach (Collider2D collider in detectedObjects)
        {
            collider.GetComponent<IDamagable>().Damage(attackDetails);
        }
    }
}
