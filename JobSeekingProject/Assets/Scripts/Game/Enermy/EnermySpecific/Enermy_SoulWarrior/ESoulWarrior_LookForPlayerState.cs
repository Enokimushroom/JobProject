using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulWarrior_LookForPlayerState : LookForPlayerState
{
    private Enermy_SoulWarrior enermy;
    private bool isPlayerInMaxAgroRange;
    private bool performInCloseActionRange;

    public ESoulWarrior_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, Enermy_SoulWarrior enermy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performInCloseActionRange)
        {
            stateMachine.ChangeState(enermy.flashDodgeState);
        }
        else if (isPlayerInMinAgroRange)
        {
            float index = Random.Range(-1.0f, 1.0f);
            if (index > 0)
            {
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enermy.teleportState);
            }
        }
        else if(isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.rangeAttackState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
