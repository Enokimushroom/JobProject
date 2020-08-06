using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELeapingHusk_JumpAttackState : JumpAttackState
{
    private Enermy_LeapingHusk enermy;

    public ELeapingHusk_JumpAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_JumpAttack stateData, Enermy_LeapingHusk enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = enermy.CheckGround();
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
                stateMachine.ChangeState(enermy.idleState);
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
