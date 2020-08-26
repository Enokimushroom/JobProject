using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBelfly_IdleState : IdleState
{
    private Enermy_Belfly enermy;

    public EBelfly_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enermy_Belfly enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
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
        if (isPlayerInMinAgroRange)
        {
            enermy.diveState.SetTargetPosition(GameObject.FindWithTag("Player").transform.position);
            stateMachine.ChangeState(enermy.playerDetectedState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
