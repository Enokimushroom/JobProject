using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHuskSentry_SpawnState : SpawnState
{
    private Enermy_HuskSentry enermy;

    public EHuskSentry_SpawnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_SpawnState stateData, Enermy_HuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
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
