using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulWarrior_DodgeState : DodgeState
{
    private Enermy_SoulWarrior enermy;
    private bool isPlayerInMinAgroRange;

    public ESoulWarrior_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData, Enermy_SoulWarrior enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

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

        if (isDodgeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(enermy.meleeAttackState);
            }
            else if (!performCloseRangeAction&&isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
            else if (!isPlayerInMinAgroRange && isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enermy.teleportState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
