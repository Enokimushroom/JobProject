using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EViolentHusk_ChargeState : ChargeState
{
    private Enermy_ViolentHusk enermy;

    public EViolentHusk_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enermy_ViolentHusk enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enermy.deathState);
        }
        else if (isChargeTimeOver)
        {
            stateMachine.ChangeState(enermy.deathState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
