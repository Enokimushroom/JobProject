using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportState : State
{
    protected D_TeleportState stateData;
    protected bool isTeleportOver;

    public TeleportState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_TeleportState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        isTeleportOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.teleportTime)
        {
            isTeleportOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
