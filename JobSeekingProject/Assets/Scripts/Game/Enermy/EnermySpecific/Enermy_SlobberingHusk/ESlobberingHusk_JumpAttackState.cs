using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESlobberingHusk_JumpAttackState : JumpAttackState
{
    private Enermy_SlobberingHusk enermy;

    public ESlobberingHusk_JumpAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_JumpAttack stateData, Enermy_SlobberingHusk enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
                stateMachine.ChangeState(enermy.playerDetectedState);
            }
            else
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
