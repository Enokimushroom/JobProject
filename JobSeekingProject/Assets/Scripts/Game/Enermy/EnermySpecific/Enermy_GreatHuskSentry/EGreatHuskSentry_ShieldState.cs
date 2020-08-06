using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGreatHuskSentry_ShieldState : ShieldState
{
    private Enermy_GreatHuskSentry enermy;

    public EGreatHuskSentry_ShieldState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ShieldState stateData, Enermy_GreatHuskSentry enermy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enermy = enermy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInShieldRange = enermy.CheckShield();
    }

    public override void Enter()
    {
        base.Enter();

        enermy.InShieldFront = true;
    }

    public override void Exit()
    {
        base.Exit();

        enermy.InShieldFront = false;
        enermy.InShieldTop = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enermy.HitFrontShield || enermy.HitTopShield)
        {
            enermy.anim.SetBool("HitFrontShield", enermy.HitFrontShield);
            enermy.anim.SetBool("HitTopShield", enermy.HitTopShield);
            stateMachine.ChangeState(enermy.meleeAttackState);
        }
        else if (performCloseRangeAction || performLongRangeAction)
        {
            stateMachine.ChangeState(enermy.meleeAttackState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enermy.moveState);
        }
        else if (!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(enermy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isPlayerInShieldRange)
        {
            if (!PlayerStatus.Instance.OnGround)
            {
                enermy.anim.SetBool("ShieldTop", true);
                enermy.InShieldFront = false;
                enermy.InShieldTop = true;
            }
            else
            {
                enermy.anim.SetBool("ShieldTop", false);
                enermy.InShieldFront = true;
                enermy.InShieldTop = false;
            }
            if ((GameObject.FindWithTag("Player").transform.position.x < enermy.aliveGO.transform.position.x && enermy.facingDirection > 0) ||
                (GameObject.FindWithTag("Player").transform.position.x > enermy.aliveGO.transform.position.x && enermy.facingDirection < 0))
            {
                entity.Flip();
            }
        }
    }
}
