using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EVengefly_ChaseState : ChaseState
{
    private Enermy_Vengefly enermy;

    public EVengefly_ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enermy_Vengefly enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        back = !isPlayerInMaxAgroRange;
        if (back && Vector2.Distance(entity.aliveGO.transform.position, entity.transform.position) <= 0.1f)
        {
            stateMachine.ChangeState(enermy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
