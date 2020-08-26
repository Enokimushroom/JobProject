using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveState : State
{
    protected D_DiveState stateData;
    protected bool isDiveTimeOver;
    protected Vector3 targetPos;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;

    public DiveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DiveState stateData) : base(entity, stateMachine, animBoolName)
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

        isDiveTimeOver = false;
        Vector2 direction = (targetPos - entity.aliveGO.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(entity.facingDirection > 0)
        {
            angle += stateData.angle;
            entity.aliveGO.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            angle += 90;
            entity.aliveGO.transform.localEulerAngles = new Vector3(0, 180, angle);
        }
        entity.SetVelocity(stateData.chargeSpeed, direction, 1);
    }

    public override void Exit()
    {
        base.Exit();
        entity.aliveGO.transform.localEulerAngles = entity.facingDirection > 0 ? Vector3.zero : new Vector3(0, 180, 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.chargeTime)
        {
            isDiveTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }
}
