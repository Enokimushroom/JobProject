using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : State
{
    protected D_SpawnState stateData;
    protected bool isSpawnTimeOver;

    public SpawnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_SpawnState stateData) : base(entity, stateMachine, animBoolName)
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

        isSpawnTimeOver = false;
        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.spawnTime)
        {
            isSpawnTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
