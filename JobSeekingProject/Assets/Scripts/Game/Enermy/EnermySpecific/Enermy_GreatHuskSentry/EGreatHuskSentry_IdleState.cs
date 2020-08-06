using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGreatHuskSentry_IdleState : IdleState
{
    private Enermy_GreatHuskSentry enermy;
    private bool isPlayerInShieldRange;
    private bool isPlayerInMaxAgroRange;

    public EGreatHuskSentry_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enermy_GreatHuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInShieldRange = enermy.CheckShield();
        isPlayerInMaxAgroRange = enermy.CheckPlayerInMaxAgroRange();
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

        if (isPlayerInShieldRange|| isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enermy.shieldState);
        }
        else if (isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.playerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
