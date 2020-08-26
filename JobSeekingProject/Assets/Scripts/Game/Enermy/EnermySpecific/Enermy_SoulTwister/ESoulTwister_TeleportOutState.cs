using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulTwister_TeleportOutState : TeleportOutState
{
    private Enermy_SoulTwister enermy;

    public ESoulTwister_TeleportOutState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_TeleportOutState stateData, Enermy_SoulTwister enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isTeleportOver)
        {
            stateMachine.ChangeState(enermy.teleportInState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
