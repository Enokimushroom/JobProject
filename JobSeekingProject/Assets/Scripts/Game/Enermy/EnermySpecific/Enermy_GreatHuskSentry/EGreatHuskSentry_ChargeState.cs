using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGreatHuskSentry_ChargeState : ChargeState
{
    private Enermy_GreatHuskSentry enermy;
    private bool isPlayerInShieldRange;

    public EGreatHuskSentry_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enermy_GreatHuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInShieldRange = enermy.CheckShield();
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

        if (isPlayerInMinAgroRange||isPlayerInShieldRange)
        {
            stateMachine.ChangeState(enermy.shieldState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            enermy.Flip();
            stateMachine.ChangeState(enermy.moveState);
        }
        else if (isChargeTimeOver&&!isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
