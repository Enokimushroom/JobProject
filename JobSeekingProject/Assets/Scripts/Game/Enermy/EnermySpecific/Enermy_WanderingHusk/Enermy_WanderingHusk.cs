using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_WanderingHusk : Entity
{
    public EWanderingHusk_IdleState idleState { get; private set; }
    public EWanderingHusk_MoveState moveState { get; private set; }
    public EWanderingHusk_DeathState deathState { get; private set; }
    public EWanderingHusk_PlayerDetectedState playerDetectedState { get; private set; }
    public EWanderingHusk_LookForPlayerState lookForPlayerState { get; private set; }
    public EWanderingHusk_ChargeState chargeState { get; private set; }


    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_DeathState deathStateData;



    public override void Start()
    {
        base.Start();

        idleState = new EWanderingHusk_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EWanderingHusk_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new EWanderingHusk_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        lookForPlayerState = new EWanderingHusk_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        chargeState = new EWanderingHusk_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        deathState = new EWanderingHusk_DeathState(this, stateMachine, "Death", deathStateData, this);

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
