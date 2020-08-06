using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_SoulWarrior : Entity
{
    public ESoulWarrior_IdleState idleState { get; private set; }
    public ESoulWarrior_MoveState moveState { get; private set; }
    public ESoulWarrior_PlayerDetectedState playerDetectedState { get; private set; }
    public ESoulWarrior_LookForPlayerState lookForPlayerState { get; private set; }
    public ESoulWarrior_ChargeState chargeState { get; private set; }
    public ESoulWarrior_TeleportState teleportState { get; private set; }
    public ESoulWarrior_StompState stompState { get; private set; } 
    public ESoulWarrior_MeleeAttackState meleeAttackState { get; private set; }
    public ESoulWarrior_RangeAttackState rangeAttackState { get; private set; }
    public ESoulWarrior_DodgeState dodgeState { get; private set; }
    public ESoulWarrior_FlashDodgeState flashDodgeState { get; private set; }
    public ESoulWarrior_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_TeleportState teleportStateData;
    [SerializeField] private D_StompState stompStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_RangeAttack rangeAttackStateData;
    [SerializeField] public D_DodgeState dodgeStateData;
    [SerializeField] public  D_DodgeState flashDodgeStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;
    [SerializeField] private Transform stompAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new ESoulWarrior_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new ESoulWarrior_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new ESoulWarrior_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        lookForPlayerState = new ESoulWarrior_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        chargeState = new ESoulWarrior_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        teleportState = new ESoulWarrior_TeleportState(this, stateMachine, "Teleport", teleportStateData, this);
        stompState = new ESoulWarrior_StompState(this, stateMachine, "Stomp", stompAttackPosition, stompStateData, this);
        meleeAttackState = new ESoulWarrior_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        rangeAttackState = new ESoulWarrior_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);
        dodgeState = new ESoulWarrior_DodgeState(this, stateMachine, "Dodge", dodgeStateData, this);
        flashDodgeState = new ESoulWarrior_FlashDodgeState(this, stateMachine, "FlashDodge", flashDodgeStateData, this);
        deathState = new ESoulWarrior_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        if (isDeath) return;

        base.Damage(attackDetails);

        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
        else if (CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangeAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(stompAttackPosition.position, stompStateData.attackRadius);
    }

}
