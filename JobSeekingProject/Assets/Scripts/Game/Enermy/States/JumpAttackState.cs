using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState : AttackState
{
    protected D_JumpAttack stateData;
    protected AttackDetails attackDetails;
    protected bool isGrounded;

    public JumpAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition,D_JumpAttack stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGO.transform.position;
        Vector2 force = AddForceCalculate.CalculateFroce(entity.aliveGO.transform, GameObject.FindWithTag("Player").transform, stateData.jumpHeight);
        entity.rb.AddForce(force, ForceMode2D.Impulse);
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

        entity.anim.SetBool("OnGround", isGrounded);
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

        foreach(Collider2D co in detectedObjects)
        {
            co.GetComponent<IDamagable>().Damage(attackDetails);
        }
    }
}
