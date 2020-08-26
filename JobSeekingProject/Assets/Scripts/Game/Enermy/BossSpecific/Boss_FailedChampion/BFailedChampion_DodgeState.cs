using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFailedChampion_DodgeState : DodgeState
{
    private Boss_FailedChampion enermy;

    public BFailedChampion_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();


    }

    public override void Enter()
    {
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoChecks();

        isDodgeOver = false;

        float index = Random.Range(-1.0f, 1.0f);
        if (index > 0)
        {
            entity.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, entity.facingDirection);
        }
        else
        {
            entity.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -entity.facingDirection);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if ((GameObject.FindWithTag("Player").transform.position.x < enermy.aliveGO.transform.position.x && enermy.facingDirection > 0) ||
                 (GameObject.FindWithTag("Player").transform.position.x > enermy.aliveGO.transform.position.x && enermy.facingDirection < 0))
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDodgeOver)
        {
            float index = Random.Range(-1.0f, 2.0f);
            if (index > 1.0f)
            {
                stateMachine.ChangeState(enermy.idleState);
            }
            else if (index <= 1.0f && index > 0)
            {
                stateMachine.ChangeState(enermy.attackState);
            }
            else
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enermy.anim.SetBool("OnGround", isGrounded);
    }
}
