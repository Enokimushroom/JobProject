using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeathState : State
{
    protected D_DeathState stateData;

    protected bool isDeathOver;
    protected bool isGrounded;

    public DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeathState stateData) : base(entity, stateMachine, animBoolName)
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

        isDeathOver = false;
        entity.transform.GetChild(0).Find("CollisionTrigger").GetComponent<Collider2D>().enabled = false;
        entity.anim.SetTrigger("Death");
        entity.aliveGO.gameObject.layer = LayerMask.NameToLayer("Pieces");
        entity.SetVelocity(stateData.deathSpeed, stateData.deathDirection, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.deathTime && isGrounded)
        {
            isDeathOver = true;
            entity.GetComponentInChildren<EnermyStatus>().DeathEvent();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public IEnumerator DissolveTime(float time,float dissolveDuration)
    {
        yield return new WaitForSeconds(time);
        entity.GetComponentInChildren<SpriteRenderer>().material.DOFloat(0, "Fade", dissolveDuration).onComplete = () => 
        {
            GameObject.Destroy(entity.gameObject);
        };
    }
}
