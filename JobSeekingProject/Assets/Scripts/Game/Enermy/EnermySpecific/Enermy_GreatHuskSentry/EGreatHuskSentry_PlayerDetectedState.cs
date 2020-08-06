using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EGreatHuskSentry_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_GreatHuskSentry enermy;
    private bool isPlayerInShieldRange;

    public EGreatHuskSentry_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_GreatHuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enermy.meleeAttackState);
        }
        else if (isPlayerInShieldRange)
        {
            stateMachine.ChangeState(enermy.shieldState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enermy.chargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.idleState);
        }
        else if (!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
