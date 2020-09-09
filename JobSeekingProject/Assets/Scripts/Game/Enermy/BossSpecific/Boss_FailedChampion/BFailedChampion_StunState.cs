using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFailedChampion_StunState : StunState
{
    private Boss_FailedChampion enermy;

    public BFailedChampion_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Boss_FailedChampion enermy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (enermy.hitDuringStun)
        {
            enermy.hitDuringStun = false;
            enermy.anim.SetTrigger("Hit");
            MusicMgr.Instance.PlaySound("FalseKnightStunHitAudio", false);
            PEManager.Instance.GetParticleObjectDuringTime("StunHitPO", entity.transform, new Vector3(-1.4f, -1.5f, 0), Vector3.one, Quaternion.identity, 0.6f);
        }
        if (isStunTimeOver)
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
}
