using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGreatHuskSentry_DeathState : DeathState
{
    private Enermy_GreatHuskSentry enermy;

    public EGreatHuskSentry_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeathState stateData, Enermy_GreatHuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isDeathOver)
        {
            isDeathOver = false;
            enermy.rb.velocity = Vector2.zero;
            enermy.StartCoroutine(DissolveTime(stateData.deathStopTime, stateData.deathDissolveTime));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.anim.SetBool("OnGround", isGrounded);
    }
}
