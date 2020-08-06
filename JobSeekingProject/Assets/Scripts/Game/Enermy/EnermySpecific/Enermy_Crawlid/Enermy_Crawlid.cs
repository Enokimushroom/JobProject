using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Crawlid : Entity
{
    public ECrawlid_IdleState idleState { get; private set; }
    public ECrawlid_MoveState moveState { get; private set; }
    public ECrawlid_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_DeathState deathStateData;

    public override void Start()
    {
        base.Start();

        idleState = new ECrawlid_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new ECrawlid_MoveState(this, stateMachine, "Move", moveStateData, this);
        deathState = new ECrawlid_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(moveState);
    }


    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);


        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }

    }
}
