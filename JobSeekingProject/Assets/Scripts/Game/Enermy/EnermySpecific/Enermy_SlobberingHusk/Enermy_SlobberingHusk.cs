using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_SlobberingHusk : Entity
{
    public ESlobberingHusk_IdleState idleState { get; private set; }
    public ESlobberingHusk_MoveState moveState { get; private set; }
    public ESlobberingHusk_PlayerDetectedState playerDetectedState { get; private set; }
    public ESlobberingHusk_LookForPlayerState lookForPlayerState { get; private set; }
    public ESlobberingHusk_JumpAttackState jumpAttackState { get; private set; }
    public ESlobberingHusk_RangeAttackState rangeAttackState { get; private set; }
    public ESlobberingHusk_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDectedStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_JumpAttack jumpAttackStateData;
    [SerializeField] private D_RangeAttack rangeAttackStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new ESlobberingHusk_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new ESlobberingHusk_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new ESlobberingHusk_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDectedStateData, this);
        lookForPlayerState = new ESlobberingHusk_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        jumpAttackState = new ESlobberingHusk_JumpAttackState(this, stateMachine, "JumpAttack", meleeAttackPosition, jumpAttackStateData, this);
        rangeAttackState = new ESlobberingHusk_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);
        deathState = new ESlobberingHusk_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(moveState);
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

        Gizmos.DrawWireSphere(meleeAttackPosition.position, jumpAttackStateData.attackRadius);
    }
}
