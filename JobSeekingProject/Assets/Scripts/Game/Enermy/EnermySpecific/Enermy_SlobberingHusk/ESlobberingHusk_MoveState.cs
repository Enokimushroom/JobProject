﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ESlobberingHusk_MoveState : MoveState
{
    private Enermy_SlobberingHusk enermy;

    public ESlobberingHusk_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enermy_SlobberingHusk enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enermy.playerDetectedState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            enermy.idleState.SetFlipAfterIdele(true);
            stateMachine.ChangeState(enermy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
