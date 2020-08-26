using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CharacterMovement : MonoBehaviour,IDamagable
{
    private float directionInput;

    private float wallJumpForce = 16;
    private float wallReactingForce = 10;
    [Header("垂直速度相关")]
    [SerializeField] float maxGravityVelocity = 20;
    [SerializeField] float jumpGravityScale = 1;
    [SerializeField] float fallGravityScale = 2.5f;
    [SerializeField] float slidingGravityScale = 2;
    [SerializeField] float groundedGravityScale = 0.5f;

    private bool canJump;
    private bool isFalling;
    private int amountOfJumpsLeft;
    private bool knockback;
    private bool canBeHurt = true;
    private float injuredStartTime;
    private float knockbackStartTime;
    [Header("受击")]
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float injuredDuration;
    [SerializeField] private Vector2 knockbackSpeed;

    #region 音效
    private bool playRunAudio;
    private AudioSource runAudio;

    private bool playSlideAudio;
    private AudioSource slideAudio;

    private Vector3 fallingStartPoint;
    private bool playFallingAudio;
    private AudioSource fallingAudio;
    #endregion

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sp;

    #region 物理检测
    [Header("物理检测")]
    public float groundCheckRadius;
    public float wallCheckDistance;
    public Transform groundCheck;
    public Transform wallCheck;
    [SerializeField] LayerMask whatIsGround;
    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        if (!GameDataMgr.Instance.playerInfo.FirstTime)
        {
            anim.Play("Respawn");
        }
        else
        {
            PlayerStatus.Instance.InputEnable = false;
        }
        PlayerStatus.Instance.IsAlive = true;
        PlayerStatus.Instance.IsFacingRight = true;
        PlayerStatus.Instance.EnableGravity = true;
    }

    private void Update()
    {
        CheckInjured();
        CheckLand();
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        UpdateGravityScale();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckMovementDirection()
    {
        if (PlayerStatus.Instance.IsFacingRight && directionInput < 0)
        {
            Flip();
        }
        else if (!PlayerStatus.Instance.IsFacingRight && directionInput > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0)
        {
            PlayerStatus.Instance.IsRunning = true;
        }
        else
        {
            PlayerStatus.Instance.IsRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("Run", PlayerStatus.Instance.IsRunning);
        anim.SetBool("OnGround", PlayerStatus.Instance.OnGround);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", PlayerStatus.Instance.IsWallSliding);
    }

    private void CheckIfCanJump()
    {
        if ((PlayerStatus.Instance.OnGround && rb.velocity.y <= 0) || PlayerStatus.Instance.IsWallSliding)
        {
            amountOfJumpsLeft = 1;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

        if (!SkillMgr.Instance.FixSkill.ContainsKey("S002")) return;
        if ((PlayerStatus.Instance.OnGround || PlayerStatus.Instance.IsWallSliding) && !PlayerStatus.Instance.CanDoubleJump)
        {
            PlayerStatus.Instance.CanDoubleJump = true;
        }
    }

    private void CheckIfWallSliding()
    {
        if (PlayerStatus.Instance.IsTouchingWall && !PlayerStatus.Instance.OnGround && rb.velocity.y <= 0 && directionInput != 0)
        {
            PlayerStatus.Instance.IsWallSliding = true;
            if (!playSlideAudio)
            {
                playSlideAudio = true;
                PEManager.Instance.GetParticleEffect("WallSlideDust", transform, Vector2.zero, Vector3.one, Quaternion.identity);
                MusicMgr.Instance.PlaySound("PlayerWallSlide", true,(o)=> { slideAudio = o; });
            }
        }
        else
        {
            PlayerStatus.Instance.IsWallSliding = false;
            if (playSlideAudio)
            {
                playSlideAudio = false;
                PEManager.Instance.BackParticleEffect("WallSlideDust");
                if(slideAudio!=null)
                    MusicMgr.Instance.StopSound(slideAudio);
            }
        }
    }

    private void CheckInput()
    {
        if (PlayerStatus.Instance.IsForzen) return;
        if (!PlayerStatus.Instance.InputEnable) return;
        if (Input.GetKey(KeyCodeMgr.Instance.Left.CurrentKey) && Input.GetKey(KeyCodeMgr.Instance.Right.CurrentKey))
        {
            directionInput = 0;
        }
        else if (Input.GetKey(KeyCodeMgr.Instance.Left.CurrentKey))
        {
            directionInput = -1;
        }
        else if (Input.GetKey(KeyCodeMgr.Instance.Right.CurrentKey))
        {
            directionInput = 1;
        }
        else
        {
            directionInput = 0;
        }
        if (Input.GetKeyDown(KeyCodeMgr.Instance.Jump.CurrentKey))
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {
        if (PlayerStatus.Instance.IsAlive)
        {
            Vector2 velocity = rb.velocity;
            if (PlayerStatus.Instance.EnableGravity)
            {
                if (PlayerStatus.Instance.IsWallSliding && directionInput != 0)
                {
                    velocity.y = Mathf.Clamp(velocity.y, -maxGravityVelocity / 2, maxGravityVelocity / 2);
                }
                else
                {
                    velocity.y = Mathf.Clamp(velocity.y, -maxGravityVelocity, maxGravityVelocity);
                }
            }
            else
            {
                velocity.y = 0;
            }
            if (!PlayerStatus.Instance.IsForzen && PlayerStatus.Instance.InputEnable)
            {
                rb.velocity = new Vector2(directionInput * PlayerStatus.Instance.Speed, velocity.y);
                //Audio
                if (directionInput != 0 && PlayerStatus.Instance.OnGround && !PlayerStatus.Instance.IsTouchingWall)
                {
                    if (!playRunAudio)
                    {
                        playRunAudio = true;
                        PEManager.Instance.GetParticleEffect("MoveDust", transform, new Vector3(0, -0.9f, 0), Vector3.one, Quaternion.Euler(new Vector3(0, 0, 90)));
                        MusicMgr.Instance.PlaySound("PlayerRun", true, (o) => { runAudio = o; });
                    }
                }
                else if (directionInput == 0 || !PlayerStatus.Instance.OnGround || PlayerStatus.Instance.IsTouchingWall)
                {
                    if (playRunAudio)
                    {
                        playRunAudio = false;
                        PEManager.Instance.BackParticleEffect("MoveDust");
                        if (runAudio != null)
                            MusicMgr.Instance.StopSound(runAudio);
                    }
                }
            }
            else
            {
                if (playRunAudio)
                {
                    playRunAudio = false;
                    PEManager.Instance.BackParticleEffect("MoveDust");
                    if (runAudio != null)
                        MusicMgr.Instance.StopSound(runAudio);
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            PlayerStatus.Instance.EnableGravity = false;

            //死亡时保证清理音效
            if (playRunAudio)
            {
                playRunAudio = false;
                PEManager.Instance.BackParticleEffect("MoveDust");
                if (runAudio != null)
                    MusicMgr.Instance.StopSound(runAudio);
            }
            if (playSlideAudio)
            {
                playSlideAudio = false;
                PEManager.Instance.BackParticleEffect("WallSlideDust");
                if (slideAudio != null)
                    MusicMgr.Instance.StopSound(slideAudio);
            }
        }
    }

    private void Jump()
    {
        if (PlayerStatus.Instance.InputEnable && canJump && !PlayerStatus.Instance.IsWallSliding)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, PlayerStatus.Instance.JumpForce), ForceMode2D.Impulse);
            amountOfJumpsLeft--;
            MusicMgr.Instance.PlaySound("PlayerJump", false);
            PEManager.Instance.GetParticleEffectDuringTime("JumpDust", 0.5f, transform, Vector2.zero, Vector3.one, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
        if (PlayerStatus.Instance.IsWallSliding && !PlayerStatus.Instance.OnGround && directionInput != 0)
        {
            StartCoroutine(WallJump());
        }
    }

    private IEnumerator WallJump()
    {
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.EnableGravity = false;
        anim.SetTrigger("ClimbJump");
        MusicMgr.Instance.PlaySound("PlayerWallJump", false);
        float force = PlayerStatus.Instance.IsFacingRight ? -wallReactingForce : wallReactingForce;
        rb.velocity = new Vector2(transform.lossyScale.x * force, wallJumpForce);
        yield return new WaitForSeconds(0.1f);
        PlayerStatus.Instance.EnableGravity = true;
        PlayerStatus.Instance.InputEnable = true;
        anim.ResetTrigger("ClimbJump");
    }

    private void UpdateGravityScale()
    {
        var gravityScale = groundedGravityScale;

        if (!PlayerStatus.Instance.OnGround)
        {
            if (PlayerStatus.Instance.IsWallSliding & directionInput != 0)
            {
                gravityScale = slidingGravityScale;
            }
            else
            {
                gravityScale = (rb.velocity.y > 0 && Input.GetKey(KeyCodeMgr.Instance.Jump.CurrentKey)) ? jumpGravityScale : fallGravityScale;
            }
        }

        if (!PlayerStatus.Instance.EnableGravity)
        {
            gravityScale = 0;
        }
        rb.gravityScale = gravityScale;
    }

    public void Flip()
    {
        if (!PlayerStatus.Instance.IsWallSliding && PlayerStatus.Instance.CanFlip)
        {
            PlayerStatus.Instance.IsFacingRight = !PlayerStatus.Instance.IsFacingRight;
            transform.Rotate(0, 180, 0);
            if (PlayerStatus.Instance.OnGround)
                anim.SetTrigger("Rotate");
        }
    }

    private void CheckLand()
    {
        if (rb.velocity.y < 0 && !PlayerStatus.Instance.OnGround && !PlayerStatus.Instance.IsWallSliding)
        {
            isFalling = true;
            if (!playFallingAudio)
            {
                playFallingAudio = true;
                fallingStartPoint = transform.position;
                MusicMgr.Instance.PlaySound("PlayerFalling", true, (o) => { fallingAudio = o; });
            }
        }
        else
        {
            if (playFallingAudio)
            {
                playFallingAudio = false;
                if (fallingAudio != null)
                    MusicMgr.Instance.StopSound(fallingAudio);
            }
        }
        if (isFalling && PlayerStatus.Instance.OnGround)
        {
            isFalling = false;
            if (fallingStartPoint.y - transform.position.y >= 10)
            {
                anim.SetBool("HardLand", true);
                MusicMgr.Instance.PlaySound("PlayerHardLand", false);
            }
            else
            {
                anim.SetBool("HardLand", false);
                MusicMgr.Instance.PlaySound("PlayerSoftLand", false);
            }
        }
    }

    private void CheckSurroundings()
    {
        PlayerStatus.Instance.OnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        PlayerStatus.Instance.IsTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckInjured()
    {
        if (Time.time >= injuredStartTime + injuredDuration * PlayerStatus.Instance.HitRecoverRate && !canBeHurt)
        {
            canBeHurt = true;
            //停止材质闪烁
            sp.material.SetFloat("_FlashAmount", 0);
        }
        else
        {
            if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
            {
                knockback = false;
                PlayerStatus.Instance.InputEnable = true;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }     
    }

    public void Damage(AttackDetails ad)
    {
        if (!PlayerStatus.Instance.IsAlive) return;
        if (!canBeHurt) return;
        canBeHurt = false;
        knockback = true;
        injuredStartTime = Time.time;
        knockbackStartTime = Time.time;
        PlayerStatus.Instance.ChangeCurrentHealth(-1 * (int)ad.damageAmount);
        if (PlayerStatus.Instance.CurrentHealth <= 0 && PlayerStatus.Instance.IsAlive)
        {
            PlayerStatus.Instance.IsAlive = false;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Death");
            PEManager.Instance.GetParticleEffectByTime("DeadAshPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, -45), 1.0f);
            PEManager.Instance.GetParticleEffectByTime("DeadAshPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(0, 0, 135), 1.0f);
            PEManager.Instance.GetParticleObjectDuringTime("DeadPO", transform, Vector3.zero, Vector3.one, Quaternion.identity, 0.5f);
            PEManager.Instance.GetParticleEffectOneOff("DeadWavePE", transform, Vector3.zero, Vector3.one, Quaternion.identity);
            MusicMgr.Instance.PlaySound("PlayerDeath", false);
            GameDataMgr.Instance.AllOBDetach();
            InputMgr.Instance.StartOrEndCheck(false);
            return; 
        }
        GetInjured(ad.position.x < transform.position.x ? 1 : -1);
        anim.SetTrigger("Hit");
        Debug.Log("受到攻击了");
    }

    private void GetInjured(int direction)
    {
        PEManager.Instance.GetParticleObjectDuringTime("HitPlayerCrackEffect", transform, Vector3.zero, Vector3.one, Quaternion.identity, 0.4f);
        PEManager.Instance.GetParticleEffectOneOff("HitPlayerParticleEffect", transform, Vector2.zero, Vector3.one, Quaternion.Euler(0, 90, 0));
        PEManager.Instance.GetParticleEffectOneOff("HitPlayerParticleEffect", transform, Vector2.zero, Vector3.one, Quaternion.Euler(0, -90, 0));
        MusicMgr.Instance.PlaySound("PlayerInjured", false);
        CinemachineShake.Instance.ShakeCamera(10f, 0.5f);
        PlayerStatus.Instance.InputEnable = false;
        rb.velocity = new Vector2(knockbackSpeed.x * direction * PlayerStatus.Instance.FallBackRate, knockbackSpeed.y);
        //材质闪烁
        StartCoroutine(InjuredFlashShader(injuredDuration * PlayerStatus.Instance.HitRecoverRate));
    }

    private IEnumerator InjuredFlashShader(float duration)
    {
        float time = 0;
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        while (time <= duration)
        {
            sp.material.SetFloat("_FlashAmount", 0.7f);
            time += 0.1f;
            yield return delay;
            sp.material.SetFloat("_FlashAmount", 0.2f);
            time += 0.1f;
            yield return delay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        int face = PlayerStatus.Instance.IsFacingRight ? 1 : -1;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * face, wallCheck.position.y, wallCheck.position.z));
    }
}
