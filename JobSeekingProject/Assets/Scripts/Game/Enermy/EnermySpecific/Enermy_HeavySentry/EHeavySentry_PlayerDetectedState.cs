using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHeavySentry_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_HeavySentry enermy;

    public EHeavySentry_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_HeavySentry enermy) : base(entity, stateMachine, animBoolName, stateData)
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enermy.meleeAttackState);
        }
        else if (performLongRangeAction)
        {
            if (Time.time >= enermy.meleeAttackState.jumpAttackStartTime + 5.0f)
            {
                stateMachine.ChangeState(enermy.meleeAttackState);
            }
            else
            {
                stateMachine.ChangeState(enermy.chargeState);
            }
        }
        else if (!isPlayerInMinAgroRange && isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.rangeAttackState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.idleState);
        }
        else if (!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
