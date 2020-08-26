using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ESoulTwister_TeleportInState : TeleportInState
{
    private Enermy_SoulTwister enermy;

    public ESoulTwister_TeleportInState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_TeleportInState stateData, Enermy_SoulTwister enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMaxAgroRange || isPlayerInMinAgroRange)
        {
            if (Time.time >= enermy.rangeAttackState.startTime + enermy.rangeAttackStateData.attackCD)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if (isTeleportInOver)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
        }
        else
        {
            if (isTeleportInOver)
                stateMachine.ChangeState(enermy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
