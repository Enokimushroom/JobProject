using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EHuskSentry_ChargeState : ChargeState
{
    private Enermy_HuskSentry enermy;

    public EHuskSentry_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enermy_HuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
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
            float index = Random.Range(-1.0f, 1.0f);
            if (index >= 0)
            {
                stateMachine.ChangeState(enermy.meleeAttackState);
            }
            else
            {
                stateMachine.ChangeState(enermy.dashAttackState);
            }
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enermy.lookForPlayerState);
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enermy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
