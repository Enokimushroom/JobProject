﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELeapingHusk_ChargeState : ChargeState
{
    private Enermy_LeapingHusk enermy;

    public ELeapingHusk_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enermy_LeapingHusk enermy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (!isDetectingLedge || isDetectingWall)
        {
            enermy.idleState.SetFlipAfterIdele(true);
            stateMachine.ChangeState(enermy.idleState);
        }
        else if (isChargeTimeOver)
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
    }
}
