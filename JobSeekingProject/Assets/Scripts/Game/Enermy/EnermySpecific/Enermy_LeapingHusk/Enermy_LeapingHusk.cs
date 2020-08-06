using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_LeapingHusk : Entity
{
    public ELeapingHusk_IdleState idleState { get; private set; }
    public ELeapingHusk_MoveState moveState { get; private set; }
    public ELeapingHusk_PlayerDetectedState playerDetectedState { get; private set; }
    public ELeapingHusk_ChargeState chargeState { get; private set; }
    public ELeapingHusk_JumpAttackState meleeAttackState { get; private set; }
    public ELeapingHusk_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_JumpAttack jumpAttackStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new ELeapingHusk_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new ELeapingHusk_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new ELeapingHusk_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDectedStateData, this);
        chargeState = new ELeapingHusk_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        meleeAttackState = new ELeapingHusk_JumpAttackState(this, stateMachine, "Attack", meleeAttackPosition,jumpAttackStateData, this);
        deathState = new ELeapingHusk_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, jumpAttackStateData.attackRadius);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
    }

    public float GetJumpHeight()
    {
        return jumpAttackStateData.jumpHeight;
    }
}
