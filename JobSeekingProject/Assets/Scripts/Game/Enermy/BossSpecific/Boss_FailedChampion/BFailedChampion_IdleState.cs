using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFailedChampion_IdleState : IdleState
{
    private Boss_FailedChampion enermy;

    public BFailedChampion_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, stateData)
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            float index = Random.Range(-2.0f, 2.0f);
            if (index > 1.0f)
            {
                stateMachine.ChangeState(enermy.jumpAttackState);
            }
            else if (index <= 1.0f && index > 0)
            {
                stateMachine.ChangeState(enermy.dodgeState);
            }
            else if (index <= 0 && index < -1.0f)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
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
    }

}
