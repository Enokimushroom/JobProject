using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BFailedChampion_JumpAttackState : JumpAttackState
{
    private Boss_FailedChampion enermy;

    public BFailedChampion_JumpAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_JumpAttack stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        if ((GameObject.FindWithTag("Player").transform.position.x < enermy.aliveGO.transform.position.x && enermy.facingDirection > 0) ||
                 (GameObject.FindWithTag("Player").transform.position.x > enermy.aliveGO.transform.position.x && enermy.facingDirection < 0))
        {
            entity.Flip();
        }

        enermy.anim.SetBool("OnGround", true);
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
            float index = Random.Range(-2.0f, 1.0f);
            if (index > 0)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if (index <= 0 && index < -1.0f)
            {
                stateMachine.ChangeState(enermy.idleState);
            }
            else
            {
                stateMachine.ChangeState(enermy.attackState);
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

    public override void JumpMode()
    {
        Vector3 targetPos = GameObject.FindWithTag("Player").transform.position;
        if (Mathf.Abs(enermy.aliveGO.transform.position.x - targetPos.x) > entity.entityData.minAgroDistance)
        {
            targetPos = enermy.aliveGO.transform.position + new Vector3(entity.entityData.minAgroDistance * entity.facingDirection, 0, 0);
        }
        Vector2 force = AddForceCalculate.CalculateFroce(entity.aliveGO.transform.position, targetPos, stateData.jumpHeight);
        entity.rb.AddForce(force, ForceMode2D.Impulse);
    }
}
