using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulTwister_IdleState : IdleState
{
    private Enermy_SoulTwister enermy;
    private bool isPlayerInMaxAgroRange;

    public ESoulTwister_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enermy_SoulTwister enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMaxAgroRange&&isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enermy.playerDetectedState);
        }
        else if(isPlayerInMinAgroRange&&!isPlayerInMaxAgroRange)
        {
            enermy.teleportInState.BackToStartRange(true);
            stateMachine.ChangeState(enermy.teleportOutState); 
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
