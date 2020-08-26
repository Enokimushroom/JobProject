using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulTwister_RangeAttackState : RangeAttackState
{
    private Enermy_SoulTwister enermy;
    private bool isPlayerInMaxAgroRange;

    public ESoulTwister_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttack stateData, Enermy_SoulTwister enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else
            {
                enermy.teleportInState.BackToStartRange(true);
                stateMachine.ChangeState(enermy.teleportOutState);
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
