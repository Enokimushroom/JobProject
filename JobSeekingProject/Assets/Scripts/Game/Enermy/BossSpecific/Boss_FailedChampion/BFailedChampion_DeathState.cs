using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFailedChampion_DeathState : DeathState
{
    private Boss_FailedChampion enermy;

    public BFailedChampion_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeathState stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, stateData)
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
        MusicMgr.Instance.PlaySound("FalseKnightDeathAudio", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
