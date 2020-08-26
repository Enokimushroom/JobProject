using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHeavySentry_MeleeAttackState : MeleeAttackState
{
    public float jumpAttackStartTime = 0;
    private Enermy_HeavySentry enermy;
    private bool isGrounded;

    public EHeavySentry_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Enermy_HeavySentry enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = enermy.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        if (enermy.CheckPlayerInCloseRangeAction())
        {
            //近战攻击
            enermy.anim.SetInteger("AttackInt", 1);
        }
        else
        {
            //跳劈
            jumpAttackStartTime = Time.time;
            enermy.anim.SetInteger("AttackInt", 2);
            Vector2 force = AddForceCalculate.CalculateFroce(enermy.aliveGO.transform.position, GameObject.FindWithTag("Player").transform.position, 3.0f);
            enermy.rb.AddForce(force, ForceMode2D.Impulse);
        }
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

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enermy.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enermy.anim.SetBool("OnGround", isGrounded);
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
