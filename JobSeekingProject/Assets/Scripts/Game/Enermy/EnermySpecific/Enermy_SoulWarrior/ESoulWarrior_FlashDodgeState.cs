﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESoulWarrior_FlashDodgeState : DodgeState
{
    private Enermy_SoulWarrior enermy;

    public ESoulWarrior_FlashDodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData, Enermy_SoulWarrior enermy) : base(entity, stateMachine, animBoolName, stateData)
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
        entity.aliveGO.transform.Find("CollisionTrigger").GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
        entity.aliveGO.transform.Find("CollisionTrigger").GetComponent<BoxCollider2D>().enabled = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDodgeOver)
        {
            stateMachine.ChangeState(enermy.stompState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
