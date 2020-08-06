using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulWarrior_StompState : StompState
{
    private Enermy_SoulWarrior enermy;
    private bool performInCloseActionRange;
    private bool isPlayerInMaxAgroRange;

    public ESoulWarrior_StompState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_StompState stateData, Enermy_SoulWarrior enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = enermy.CheckPlayerInMaxAgroRange();
        performInCloseActionRange = enermy.CheckPlayerInCloseRangeAction();
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
            if (performInCloseActionRange)
            {
                stateMachine.ChangeState(enermy.dodgeState);
            }
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else if (!isPlayerInMinAgroRange && isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enermy.lookForPlayerState);
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
