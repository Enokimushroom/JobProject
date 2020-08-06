using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_HuskDandy : Entity
{
    public EHuskDandy_IdleState idleState { get; private set; }
    public EHuskDandy_MoveState moveState { get; private set; }
    public EHuskDandy_PlayerDetectedState playerDetectedState { get; private set; }
    public EHuskDandy_ChargeState chargeState { get; private set; }
    public EHuskDandy_MeleeAttackState meleeAttackState { get; private set; }
    public EHuskDandy_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new EHuskDandy_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EHuskDandy_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new EHuskDandy_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDectedStateData, this);
        chargeState = new EHuskDandy_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        meleeAttackState = new EHuskDandy_MeleeAttackState(this, stateMachine, "Attack", meleeAttackPosition, meleeAttackStateData, this);
        deathState = new EHuskDandy_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(moveState);
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
