using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_GreatHuskSentry : Entity
{
    public EGreatHuskSentry_IdleState idleState { get; private set; }
    public EGreatHuskSentry_MoveState moveState { get; private set; }
    public EGreatHuskSentry_PlayerDetectedState playerDetectedState { get; private set; }
    public EGreatHuskSentry_ChargeState chargeState { get; private set; }
    public EGreatHuskSentry_ShieldState shieldState { get; private set; }
    public EGreatHuskSentry_MeleeAttackState meleeAttackState { get; private set; }
    public EGreatHuskSentry_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDected playerDetectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_ShieldState shieldStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public bool InShieldFront { get; set; }
    public bool InShieldTop { get; set; }
    public bool HitFrontShield { get; set; }
    public bool HitTopShield { get; set; }

    public override void Start()
    {
        base.Start();

        idleState = new EGreatHuskSentry_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EGreatHuskSentry_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new EGreatHuskSentry_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        chargeState = new EGreatHuskSentry_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        shieldState = new EGreatHuskSentry_ShieldState(this, stateMachine, "Shield", shieldStateData, this);
        meleeAttackState = new EGreatHuskSentry_MeleeAttackState(this, stateMachine, "Attack", meleeAttackPosition, meleeAttackStateData, this);
        deathState = new EGreatHuskSentry_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(moveState);
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);

        Gizmos.DrawWireCube(new Vector3(transform.Find("Alive").transform.position.x, transform.Find("Alive").transform.position.y + shieldStateData.offsetY, 0), new Vector3(shieldStateData.checkX, shieldStateData.checkY, 0));
    }

    public bool CheckShield()
    {
        return CheckShield(shieldStateData.offsetY, shieldStateData.checkX, shieldStateData.checkY, shieldStateData.whatIsPlayer);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        if (InShieldFront && ((attackDetails.position.x < aliveGO.transform.position.x && facingDirection < 0) || (attackDetails.position.x > aliveGO.transform.position.x && facingDirection > 0)) && attackDetails.type == SkillAttackType.Sword)
        {
            HitFrontShield = true;
            return;
        }
        if (InShieldTop && (attackDetails.position.y > aliveGO.transform.position.y) && attackDetails.type == SkillAttackType.Sword)
        {
            HitTopShield = true;
            return;
        }

        base.Damage(attackDetails);

        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
    }
}
