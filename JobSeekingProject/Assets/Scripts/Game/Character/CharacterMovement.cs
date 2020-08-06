using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour,IDamagable
{
    private float directionInput;
    private float variableJumpHeightMultiplier = 0.95f;

    private Rigidbody2D rb;
    private Animator anim;

    private float wallJumpForce = 16;
    private float wallReactingForce = 10;
    [SerializeField] float maxGravityVelocity = 20;
    [SerializeField] float jumpGravityScale = 1;
    [SerializeField] float fallGravityScale = 2.5f;
    [SerializeField] float slidingGravityScale = 2;
    [SerializeField] float groundedGravityScale = 0.5f;

    private bool canJump;
    private int amountOfJumpsLeft;

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
        PlayerStatus.Instance.EnableGravity = true;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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
        if (PlayerStatus.Instance.IsTouchingWall && !PlayerStatus.Instance.OnGround && rb.velocity.y <= 0)
        {
            PlayerStatus.Instance.IsWallSliding = true;
        }
        else
        {
            PlayerStatus.Instance.IsWallSliding = false;
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

        if (Input.GetKeyUp(KeyCodeMgr.Instance.Jump.CurrentKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
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
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            PlayerStatus.Instance.EnableGravity = false;
        }
    }

    private void Jump()
    {
        if (PlayerStatus.Instance.InputEnable && canJump && !PlayerStatus.Instance.IsWallSliding)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, PlayerStatus.Instance.JumpForce), ForceMode2D.Impulse);
            amountOfJumpsLeft--;
        }
        if (PlayerStatus.Instance.IsWallSliding && !PlayerStatus.Instance.OnGround)
        {
            StartCoroutine(WallJump());
        }
    }

    private IEnumerator WallJump()
    {
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.EnableGravity = false;
        anim.SetTrigger("ClimbJump");
        rb.velocity = new Vector2(transform.lossyScale.x * wallReactingForce, wallJumpForce);
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
                gravityScale = rb.velocity.y > 0 ? jumpGravityScale : fallGravityScale;
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

    private void CheckSurroundings()
    {
        PlayerStatus.Instance.OnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        PlayerStatus.Instance.IsTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    public void Damage(AttackDetails ad)
    {
        Debug.Log("受到攻击了");
        anim.SetTrigger("Hit");
        PlayerStatus.Instance.ChangeCurrentHealth(-1 * (int)ad.damageAmount);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        int face = PlayerStatus.Instance.IsFacingRight ? 1 : -1;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * face, wallCheck.position.y, wallCheck.position.z));
    }
}
