using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_HuskSentry : Entity
{
    public EHuskSentry_SpawnState spawnState { get; private set; }
    public EHuskSentry_IdleState idleState { get; private set; }
    public EHuskSentry_MoveState moveState { get; private set; }
    public EHuskSentry_PlayerDetectedState playerDetectedState { get; private set; }
    public EHuskSentry_LookForPlayerState lookForPlayerState { get; private set; }
    public EHuskSentry_ChargeState chargeState { get; private set; }
    public EHuskSentry_MeleeAttackState meleeAttackState { get; private set; }
    public EHuskSentry_DashAttackState dashAttackState { get; private set; }
    public EHuskSentry_DodgeState dodgeState { get; private set; }
    public EHuskSentry_DeathState deathState { get; private set; }

    [SerializeField] private D_SpawnState spawnStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_DashAttack dashAttackStateData;
    [SerializeField] public D_DodgeState dodgeStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform dashAttackPosition;

    public override void Start()
    {
        base.Start();

        spawnState = new EHuskSentry_SpawnState(this, stateMachine, "Spawn", spawnStateData, this);
        idleState = new EHuskSentry_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EHuskSentry_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new EHuskSentry_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        lookForPlayerState = new EHuskSentry_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        chargeState = new EHuskSentry_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        meleeAttackState = new EHuskSentry_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        dashAttackState = new EHuskSentry_DashAttackState(this, stateMachine, "DashAttack", dashAttackPosition, dashAttackStateData, this);
        dodgeState = new EHuskSentry_DodgeState(this, stateMachine, "Dodge", dodgeStateData, this);
        deathState = new EHuskSentry_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(spawnState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
        else if (CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(playerDetectedState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(dashAttackPosition.position, dashAttackStateData.attackRadius);
    }

}
