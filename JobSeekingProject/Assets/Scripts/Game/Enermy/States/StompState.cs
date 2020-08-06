using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompState : AttackState
{
    protected D_StompState stateData;
    protected bool isGrounded;
    protected float gravity;
    protected bool changeGravity;

    public StompState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_StompState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        isGrounded = false;
        changeGravity = false;
        gravity = entity.rb.gravityScale;
        entity.rb.gravityScale = 0;
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
        entity.aliveGO.transform.position = new Vector2(playerPos.x, playerPos.y + stateData.height);
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

        if (Time.time >= startTime + stateData.airHoldingTime && !changeGravity)
        {
            changeGravity = true;
            entity.rb.gravityScale = gravity;
            entity.SetVirtualVelocity(stateData.stompDownSpeed);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        entity.anim.SetBool("OnGround", isGrounded);
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
