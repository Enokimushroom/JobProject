using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHuskSentry_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_HuskSentry enermy;

    public EHuskSentry_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_HuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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
            if (Time.time >= enermy.dodgeState.startTime + enermy.dodgeStateData.dodgeCoolDown)
            {
                stateMachine.ChangeState(enermy.dodgeState);
            }
            else
            {
                float index = Random.Range(-1.0f, 1.0f);
                if (index >= 0)
                {
                    stateMachine.ChangeState(enermy.dashAttackState);
                }
                else
                {
                    stateMachine.ChangeState(enermy.meleeAttackState);
                }
            }
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enermy.chargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
