using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBelfly_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_Belfly enermy;

    public EBelfly_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_Belfly enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (performLongRangeAction)
        {
            stateMachine.ChangeState(enermy.diveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
