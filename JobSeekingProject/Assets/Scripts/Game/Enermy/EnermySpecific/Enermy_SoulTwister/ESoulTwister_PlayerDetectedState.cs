using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulTwister_PlayerDetectedState : PlayerDetectedState
{
    private Enermy_SoulTwister enermy;

    public ESoulTwister_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDected stateData, Enermy_SoulTwister enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        isPlayerInMaxAgroRange = enermy.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = enermy.CheckPlayerInMinAgroRange();
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
        if (!isPlayerInMaxAgroRange&&!isPlayerInMinAgroRange)
        {
            enermy.teleportInState.BackToStartRange(true);
            stateMachine.ChangeState(enermy.teleportOutState);
        }
        else if (!isPlayerInMaxAgroRange && isPlayerInMinAgroRange)
        {
            if (Time.time >= enermy.rangeAttackState.startTime + enermy.rangeAttackStateData.attackCD)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if(performLongRangeAction)
            {
                enermy.teleportInState.BackToStartRange(true);
                stateMachine.ChangeState(enermy.teleportOutState);
            }
        }
        else if (isPlayerInMaxAgroRange)
        {
            if (Time.time >= enermy.rangeAttackState.startTime + enermy.rangeAttackStateData.attackCD)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if (performLongRangeAction)
            {
                stateMachine.ChangeState(enermy.teleportOutState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
