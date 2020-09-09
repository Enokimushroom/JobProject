using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EViolentHusk_DeathState : DeathState
{
    private Enermy_ViolentHusk enermy;

    public EViolentHusk_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeathState stateData, Enermy_ViolentHusk enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        PEManager.Instance.GetParticleObjectDuringTime("EnermyBoomAttackPO", null, enermy.aliveGO.transform.position, Vector3.one, Quaternion.identity, 1.0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
