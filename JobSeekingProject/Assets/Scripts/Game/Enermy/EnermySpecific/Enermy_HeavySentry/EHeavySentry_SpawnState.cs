using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHeavySentry_SpawnState : SpawnState
{
    private Enermy_HeavySentry enermy;

    public EHeavySentry_SpawnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_SpawnState stateData, Enermy_HeavySentry enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isSpawnTimeOver)
        {
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
