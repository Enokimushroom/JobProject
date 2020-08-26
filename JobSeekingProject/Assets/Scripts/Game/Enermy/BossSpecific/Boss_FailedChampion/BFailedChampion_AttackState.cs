using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFailedChampion_AttackState : MeleeAttackState
{
    private Boss_FailedChampion enermy;

    public BFailedChampion_AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

        enermy.Invoke("GenerateFallBarrel", 1.2f);
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
            float index = Random.Range(-1.0f, 2.0f);
            if (index > 1.0f)
            {
                stateMachine.ChangeState(enermy.jumpAttackState);
            }
            else if (index <= 1.0f && index > 0)
            {
                stateMachine.ChangeState(enermy.dodgeState);
            }
            else
            {
                stateMachine.ChangeState(enermy.rangeAttackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); 
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D ground = Physics2D.OverlapCircle(attackPosition.position, stateData.attackRadius, entity.entityData.whatIsGround);
        RaycastHit2D hit = Physics2D.Raycast(attackPosition.position, Vector2.down, stateData.attackRadius * 2, LayerMask.GetMask("Ground"));

        if (ground)
        {
            PEManager.Instance.GetParticleObjectDuringTime("FailChampionShockwave", null, hit.point, Vector3.one, Quaternion.identity, 0.5f);
            PEManager.Instance.GetParticleEffectOneOff("FailChampionDustPE", null, hit.point, Vector3.one, Quaternion.Euler(0, 0, 90));
            PEManager.Instance.GetParticleEffectOneOff("FailChampionRocksBurst", null, hit.point, Vector3.one, Quaternion.identity);
        }
    }

}
