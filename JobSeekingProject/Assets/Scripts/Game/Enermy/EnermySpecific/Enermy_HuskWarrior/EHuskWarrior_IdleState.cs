using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHuskWarrior_IdleState : IdleState
{
    private Enermy_HuskWarrior enermy;
    private bool isPlayerInShieldRange;

    public EHuskWarrior_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enermy_HuskWarrior enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMinAgroRange || isPlayerInShieldRange)
        {
            stateMachine.ChangeState(enermy.shieldState);
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
