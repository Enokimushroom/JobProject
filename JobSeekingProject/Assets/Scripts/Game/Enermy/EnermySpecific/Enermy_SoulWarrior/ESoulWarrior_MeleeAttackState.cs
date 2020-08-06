using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ESoulWarrior_MeleeAttackState : MeleeAttackState
{
    private Enermy_SoulWarrior enermy;
    private bool isPlayerInMaxAgroRange;
    private bool peroformInCloseActionRange;

    public ESoulWarrior_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Enermy_SoulWarrior enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = enermy.CheckPlayerInMaxAgroRange();
        peroformInCloseActionRange = enermy.CheckPlayerInCloseRangeAction();
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
            if (isPlayerInMinAgroRange)
            {
                if (peroformInCloseActionRange)
                {
                    if (Time.time >= enermy.flashDodgeState.startTime + enermy.flashDodgeStateData.dodgeCoolDown)
                    {
                        stateMachine.ChangeState(enermy.flashDodgeState);
                    }
                    else
                    {
                        stateMachine.ChangeState(enermy.dodgeState);
                    }
                }
                else
                {
                    stateMachine.ChangeState(enermy.teleportState);
                }
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
