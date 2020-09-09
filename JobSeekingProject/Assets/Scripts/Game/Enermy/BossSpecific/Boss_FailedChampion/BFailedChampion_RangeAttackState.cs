using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFailedChampion_RangeAttackState : RangeAttackState
{
    private Boss_FailedChampion enermy;
    private int index;

    public BFailedChampion_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttack stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
        index = Random.Range(1, 5);
        MusicMgr.Instance.PlaySound("FalseKnightAttackAudio0" + index, false);
        MusicMgr.Instance.PlaySound("FalseKnightStrikeAudio", false);
    }

    public override void Exit()
    {
        base.Exit(); 
        
        if ((GameObject.FindWithTag("Player").transform.position.x < enermy.aliveGO.transform.position.x && enermy.facingDirection > 0) ||
                 (GameObject.FindWithTag("Player").transform.position.x > enermy.aliveGO.transform.position.x && enermy.facingDirection < 0))
        {
            entity.Flip();
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            float index = Random.Range(-2.0f, 2.0f);
            if (index > 1.0f)
            {
                stateMachine.ChangeState(enermy.jumpAttackState);
            }
            else if (index <= 1.0f && index > 0)
            {
                stateMachine.ChangeState(enermy.dodgeState);
            }
            else if (index <= 0 && index > -1.0f)
            {
                stateMachine.ChangeState(enermy.attackState);
            }
            else
            {
                stateMachine.ChangeState(enermy.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if ((GameObject.FindWithTag("Player").transform.position.x < enermy.aliveGO.transform.position.x && enermy.facingDirection > 0) ||
                (GameObject.FindWithTag("Player").transform.position.x > enermy.aliveGO.transform.position.x && enermy.facingDirection < 0))
        {
            entity.Flip();
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        MusicMgr.Instance.PlaySound("FalseKnightStrikeGround", false);
    }
}
