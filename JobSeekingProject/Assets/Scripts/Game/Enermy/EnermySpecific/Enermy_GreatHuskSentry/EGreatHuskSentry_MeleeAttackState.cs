using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGreatHuskSentry_MeleeAttackState : MeleeAttackState
{
    private Enermy_GreatHuskSentry enermy;
    private bool isPlayerInShieldRange;

    public EGreatHuskSentry_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Enermy_GreatHuskSentry enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

        enermy.HitFrontShield = false;
        enermy.HitTopShield = false;
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange || isPlayerInShieldRange)
            {
                stateMachine.ChangeState(enermy.shieldState);
            }
            else
            {
                stateMachine.ChangeState(enermy.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
