﻿using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWanderingHusk_MoveState : MoveState
{
    private Enermy_WanderingHusk enermy;
    public EWanderingHusk_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enermy_WanderingHusk enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
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
        else if (isDetectingWall || !isDetectingLedge)
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
