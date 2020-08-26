using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    protected D_MoveState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool back;
    protected Vector3 direction;

    public ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName,D_MoveState stateData) : base(entity, stateMachine, animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckFlip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        direction = back ? (entity.transform.position - entity.aliveGO.transform.position) : (GameObject.FindWithTag("Player").transform.position - entity.aliveGO.transform.position);
        direction.Normalize();

        entity.rb.MovePosition(entity.aliveGO.transform.position + direction * stateData.movementSpeed * Time.fixedDeltaTime);
    }

    public void BackToStartRange(bool back)
    {
        this.back = back;
    }

    public void CheckFlip()
    {
        if ((entity.aliveGO.transform.position.x < GameObject.FindWithTag("Player").transform.position.x && entity.facingDirection < 0) ||
            (entity.aliveGO.transform.position.x > GameObject.FindWithTag("Player").transform.position.x && entity.facingDirection > 0))
        {
            entity.Flip();
        }
    }
}
