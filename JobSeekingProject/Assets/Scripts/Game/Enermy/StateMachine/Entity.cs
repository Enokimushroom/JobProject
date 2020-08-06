using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour,IDamagable
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public int lastDamageDirection { get; private set; }

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    private Vector2 velocityWorkspace;
    
    protected bool isStunned;
    protected bool isDeath;

    public virtual void Start()
    {
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;

        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVirtualVelocity(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, -1 * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckShield(float offsetY,float sizeX,float sizeY,LayerMask layer)
    {
        return Physics2D.OverlapBox(new Vector2(aliveGO.transform.position.x, aliveGO.transform.position.y + offsetY), new Vector2(sizeX, sizeY), 0, layer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual IEnumerator DamgeHop(float velocity, int direction)
    {
        float temp = rb.velocity.x;
        Vector2 force = new Vector2(velocity * direction, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);
        rb.velocity = new Vector2(temp, 0);
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;

        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        if (attackDetails.position.x > aliveGO.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }

        if (attackDetails.type == SkillAttackType.Magic)
        {
            Vector3 scale = new Vector3(PlayerStatus.Instance.IsFacingRight ? 1 : -1, 1, 1);
            PEManager.Instance.GetParticleObjectDuringTime("HitEnermyMagicEffect", aliveGO.transform, Vector3.zero, scale, Quaternion.identity, 0.5f);
        }
        else
        {
            Vector3 scale = new Vector3(PlayerStatus.Instance.IsFacingRight ? 1 : -1, 1, 1);
            PEManager.Instance.GetParticleObjectDuringTime("HitEnermySwordEffect", aliveGO.transform, Vector3.zero, scale, Quaternion.identity, 0.5f);
        }
        PEManager.Instance.GetParticleObjectDuringTime("HitEnermyOrangeEffect", transform.Find("Alive"), Vector3.zero, Vector3.one, Quaternion.identity, 0.5f);
        PEManager.Instance.GetParticleEffectOneOff("HitEnermyBoomEffect", transform.Find("Alive"), Vector3.zero, Vector3.one, Quaternion.identity);
        PEManager.Instance.GetParticleEffectOneOff("HitEnermyExplodeEffect", transform.Find("Alive"), Vector3.zero, new Vector3(lastDamageDirection, 1, 1), Quaternion.identity);

        StartCoroutine(DamgeHop(entityData.damageHopSpeed, lastDamageDirection));

        if (currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if (currentHealth <= 0)
        {
            isDeath = true;
        }
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);
    }

}
