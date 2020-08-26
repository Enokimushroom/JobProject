using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Belfly : FlyingEntity
{
    public EBelfly_IdleState idleState { get; private set; }
    public EBelfly_PlayerDetectedState playerDetectedState { get; private set; }
    public EBelfly_DiveState diveState { get; private set; }
    public EBelfly_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_DiveState diveStateData;
    [SerializeField] private D_DeathState deathStateData;

    public override void Start()
    {
        base.Start();

        idleState = new EBelfly_IdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new EBelfly_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        diveState = new EBelfly_DiveState(this, stateMachine, "Dive", diveStateData, this);
        deathState = new EBelfly_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(idleState);
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
