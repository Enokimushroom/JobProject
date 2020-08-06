using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Steps;

public class ESoulWarrior_ChargeState : ChargeState
{
    private Enermy_SoulWarrior enermy;
    private bool isPlayerInMaxAgroRange;

    public ESoulWarrior_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enermy_SoulWarrior enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

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

        if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enermy.lookForPlayerState);
        }
        else if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enermy.meleeAttackState);
        }
        else if (isChargeTimeOver)
        {
            if (performCloseRangeAction)
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
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if (!isPlayerInMinAgroRange && isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enermy.meleeAttackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
