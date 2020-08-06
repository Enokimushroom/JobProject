using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_ViolentHusk : Entity
{
    public EViolentHusk_IdleState idleState { get; private set; }
    public EViolentHusk_MoveState moveState { get; private set; }
    public EViolentHusk_PlayerDetectedState playerDetectedState { get; private set; }
    public EViolentHusk_ChargeState chargeState { get; private set; }
    public EViolentHusk_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_DeathState deathStateData;

    public override void Start()
    {
        base.Start();

        idleState = new EViolentHusk_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EViolentHusk_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new EViolentHusk_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDectedStateData, this);
        chargeState = new EViolentHusk_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        deathState = new EViolentHusk_DeathState(this, stateMachine, "Death", deathStateData, this);

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
