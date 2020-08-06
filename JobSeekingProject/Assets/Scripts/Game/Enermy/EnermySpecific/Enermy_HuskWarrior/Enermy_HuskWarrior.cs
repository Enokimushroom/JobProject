using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_HuskWarrior : Entity
{
    public EHuskWarrior_IdleState idleState { get; private set; }
    public EHuskWarrior_MoveState moveState { get; private set; }
    public EHuskWarrior_ShieldState shieldState { get; private set; }
    public EHuskWarrior_MeleeAttackState meleeAttackState { get; private set; }
    public EHuskWarrior_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
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

        idleState = new EHuskWarrior_IdleState(this, stateMachine, "Idle", idleStateData, this);
        moveState = new EHuskWarrior_MoveState(this, stateMachine, "Move", moveStateData, this);
        shieldState = new EHuskWarrior_ShieldState(this, stateMachine, "Shield", shieldStateData, this);
        meleeAttackState = new EHuskWarrior_MeleeAttackState(this, stateMachine, "Attack", meleeAttackPosition, meleeAttackStateData, this);
        deathState = new EHuskWarrior_DeathState(this, stateMachine, "Death", deathStateData, this);

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
        if (InShieldFront && ((attackDetails.position.x < aliveGO.transform.position.x && facingDirection < 0) || (attackDetails.position.x > aliveGO.transform.position.x && facingDirection > 0))&&attackDetails.type==SkillAttackType.Sword)
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
