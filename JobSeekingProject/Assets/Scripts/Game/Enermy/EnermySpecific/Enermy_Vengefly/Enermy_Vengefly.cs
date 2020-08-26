using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Vengefly : FlyingEntity
{
    public EVengefly_IdleState idleState { get; private set; }  
    public EVengefly_ChaseState chaseState { get; private set; }
    public EVengefly_PlayerDetectedState playerDetectedState { get; private set; }
    public EVengefly_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState chaseStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_DeathState deathStateData;

    public override void Start()
    {
        base.Start();

        idleState = new EVengefly_IdleState(this, stateMachine, "Idle", idleStateData, this);
        chaseState = new EVengefly_ChaseState(this, stateMachine, "Chase", chaseStateData, this);
        playerDetectedState = new EVengefly_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        deathState = new EVengefly_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        if (isDeath) return;

        base.Damage(attackDetails);

        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
    }

}
