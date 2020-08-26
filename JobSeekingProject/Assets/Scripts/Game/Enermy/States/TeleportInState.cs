using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInState : State
{
    protected D_TeleportInState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isTeleportInOver;
    protected bool back;

    public TeleportInState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_TeleportInState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        if (!back)
        {
            Vector3 targetPos = new Vector3(Random.Range(0, 1.0f), Random.Range(0, 1.0f), 10);
            targetPos = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().ViewportToWorldPoint(targetPos);
            entity.aliveGO.transform.position = targetPos;
        }
        else
        {
            entity.aliveGO.transform.position = entity.transform.position;
            back = false;
        }
        isTeleportInOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.animTime)
        {
            isTeleportInOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void BackToStartRange(bool back)
    {
        this.back = back;
    }
}
