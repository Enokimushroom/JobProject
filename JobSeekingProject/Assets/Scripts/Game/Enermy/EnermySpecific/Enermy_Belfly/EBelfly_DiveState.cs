using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBelfly_DiveState : DiveState
{
    private Enermy_Belfly enermy;
    private bool isGrounded;

    public EBelfly_DiveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DiveState stateData, Enermy_Belfly enermy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded)
        {
            stateMachine.ChangeState(enermy.deathState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
