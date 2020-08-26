using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVengefly_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_Vengefly enermy;

    public EVengefly_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_Vengefly enermy) : base(entity, stateMachine, animBoolName, stateData)
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
            stateMachine.ChangeState(enermy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
