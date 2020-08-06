using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : State
{
    protected D_ShieldState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    protected bool isPlayerInShieldRange;

    public ShieldState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ShieldState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isDetectingLedge = entity.CheckLedge();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInShieldRange = entity.CheckShield(stateData.offsetY, stateData.checkX, stateData.checkY, stateData.whatIsPlayer);
    }

    public override void Enter()
    {
        base.Enter();
        performLongRangeAction = false;
        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.shieldTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
