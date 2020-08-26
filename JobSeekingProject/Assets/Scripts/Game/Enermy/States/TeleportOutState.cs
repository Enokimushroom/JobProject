using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOutState : State
{
    protected D_TeleportOutState stateData;
    protected bool isTeleportOver;

    public TeleportOutState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_TeleportOutState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        entity.aliveGO.transform.Find("CollisionTrigger").GetComponent<BoxCollider2D>().enabled = false;
        isTeleportOver = false;
    }

    public override void Exit()
    {
        base.Exit();
        entity.aliveGO.transform.Find("CollisionTrigger").GetComponent<BoxCollider2D>().enabled = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.teleportTime)
        {
            isTeleportOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
