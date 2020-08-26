using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FailedChampion : Entity
{
    public BFailedChampion_IdleState idleState { get; private set; }
    public BFailedChampion_DodgeState dodgeState { get; private set; }
    public BFailedChampion_JumpAttackState jumpAttackState { get; private set; }
    public BFailedChampion_RangeAttackState rangeAttackState { get; private set; }  
    public BFailedChampion_AttackState attackState { get; private set; }
    public BFailedChampion_StunState stunState { get; private set; }
    public BFailedChampion_DeathState deathState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_DodgeState dodgeStateData;
    [SerializeField] private D_JumpAttack jumpAttackStateData;
    [SerializeField] private D_RangeAttack rangeAttackStateData;
    [SerializeField] private D_MeleeAttack attackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeathState deathStateData;

    [SerializeField] private Transform rangeAttackPosition;
    [SerializeField] private Transform jumpAttackPosition;
    [SerializeField] private Transform attackPosition;


    public bool hitDuringStun { get; set; }

    public override void Start()
    {
        base.Start();

        idleState = new BFailedChampion_IdleState(this, stateMachine, "Idle", idleStateData, this);
        dodgeState = new BFailedChampion_DodgeState(this, stateMachine, "Dodge", dodgeStateData, this);
        jumpAttackState = new BFailedChampion_JumpAttackState(this, stateMachine, "JumpAttack", jumpAttackPosition, jumpAttackStateData, this);
        rangeAttackState = new BFailedChampion_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);
        attackState = new BFailedChampion_AttackState(this, stateMachine, "Attack", attackPosition, attackStateData, this);
        stunState = new BFailedChampion_StunState(this, stateMachine, "Stun", stunStateData, this);
        deathState = new BFailedChampion_DeathState(this, stateMachine, "Death", deathStateData, this);

        stateMachine.Init(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isStunned)
            hitDuringStun = true;

        if (isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
        else if (isStunned)
        {
            if(stateMachine.currentState != stunState)
            {
                stateMachine.ChangeState(stunState);
            }
            hitDuringStun = true;
        }
    }

    public override void DamageSound()
    {
        MusicMgr.Instance.PlaySound("FalseKnightDamageAudio", false);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(attackPosition.position, attackStateData.attackRadius);
        Gizmos.DrawWireSphere(jumpAttackPosition.position, jumpAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(groundCheck.position, entityData.groundCheckRadius);
    }

    private void GenerateFallBarrel()
    {
        float index = Random.Range(0, 0.2f);
        Vector3[] generatePos = new Vector3[5];
        for (int i = 0; i < generatePos.Length; ++i)
        {
            float interval = Random.Range(0, 0.1f);
            Vector3 viewPoint = new Vector3((index + 0.2f * i), (0.9f - interval), 10);
            generatePos[i] = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().ViewportToWorldPoint(viewPoint);
        }
        foreach (Vector3 v in generatePos)
        {
            PEManager.Instance.GetParticleObject("FallBarrel", null, v);
        }
    }
}
