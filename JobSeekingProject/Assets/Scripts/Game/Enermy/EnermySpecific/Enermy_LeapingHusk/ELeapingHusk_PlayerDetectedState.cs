﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELeapingHusk_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_LeapingHusk enermy;

    public ELeapingHusk_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_LeapingHusk enermy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(enermy.chargeState);
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
