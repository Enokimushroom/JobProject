using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_SoulTwister : FlyingEntity
{
    public ESoulTwister_IdleState idleState { get; private set; }
    public ESoulTwister_RangeAttackState rangeAttackState { get; private set; }
    public ESoulTwister_TeleportInState teleportInState { get; private set; }
    public ESoulTwister_TeleportOutState teleportOutState { get; private set; }

    public ESoulTwister_PlayerDetectedState playerDetectedState { get; private set; }
    public ESoulTwister_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] public D_RangeAttack rangeAttackStateData;
    [SerializeField] private D_TeleportInState teleportInStateData;
    [SerializeField] private D_TeleportOutState teleportOutStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform rangeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new ESoulTwister_IdleState(this, stateMachine, "Idle", idleStateData, this);
        rangeAttackState = new ESoulTwister_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);
        teleportInState = new ESoulTwister_TeleportInState(this, stateMachine, "TeleportIn", teleportInStateData, this);
        teleportOutState = new ESoulTwister_TeleportOutState(this, stateMachine, "TeleportOut", teleportOutStateData, this);
        playerDetectedState = new ESoulTwister_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        deathState = new ESoulTwister_DeathState(this, stateMachine, "Death", deathStateData, this);

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
