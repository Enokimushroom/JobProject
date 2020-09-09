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
        //entity.aliveGO.gameObject.layer = LayerMask.NameToLayer("Pieces");
        entity.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
        entity.SetVelocity(stateData.deathSpeed, stateData.deathDirection, entity.lastDamageDirection);
        entity.GetComponentInChildren<EnermyStatus>().DeathEvent();
        entity.StartCoroutine(SwitchLayer());
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
        }

        if (isDeathOver)
        {
            isDeathOver = false;
            entity.rb.velocity = Vector2.zero;
            entity.StartCoroutine(DissolveTime(stateData.deathStopTime, stateData.deathDissolveTime));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        entity.anim.SetBool("OnGround", isGrounded);
    }

    private IEnumerator DissolveTime(float time,float dissolveDuration)
    {
        yield return new WaitForSeconds(time);
        entity.GetComponentInChildren<SpriteRenderer>().material.DOFloat(0, "Fade", dissolveDuration).onComplete = () => 
        {
            GameObject.Destroy(entity.gameObject);
        };
    }

    private IEnumerator SwitchLayer()
    {
        yield return new WaitForSeconds(0.25f);
        entity.aliveGO.gameObject.layer = LayerMask.NameToLayer("Pieces");
    }

    
}
