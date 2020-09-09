using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_HeavySentry : Entity
{
    public EHeavySentry_SpawnState spawnState { get; private set; }
    public EHeavySentry_IdleState idleState { get; private set; }
    public EHeavySentry_MoveState moveState { get; private set; }
    public EHeavySentry_ChargeState chargeState { get; private set; }
    public EHeavySentry_PlayerDetectedState playerDetectedState { get; private set; }
    public EHeavySentry_MeleeAttackState meleeAttackState { get; private set; }
    public EHeavySentry_RangeAttackState rangeAttackState { get; private set; }
    public EHeavySentry_DeathState deathState { get; private set; }

    [SerializeField] private D_SpawnState spawnStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_ChargeState rangeAttackStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();
        spawnState = new EHeavySentry_SpawnState(this, stateMachine, "Spawn", spawnStateData, this);
        idleState = new EHeavySentry_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EHeavySentry_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new EHeavySentry_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        chargeState = new EHeavySentry_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        rangeAttackState = new EHeavySentry_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackStateData, this);
        meleeAttackState = new EHeavySentry_MeleeAttackState(this, stateMachine, "Attack", meleeAttackPosition, meleeAttackStateData, this);
        deathState = new EHeavySentry_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(spawnState);
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
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
