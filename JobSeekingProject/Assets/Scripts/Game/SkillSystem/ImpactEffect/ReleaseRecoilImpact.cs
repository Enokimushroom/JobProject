using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReleaseRecoilImpact : IImpactEffect
{
    private Rigidbody2D rb;

    public void Execute(Deployer deployer)
    {
        if (!deployer.SkillData.hasReleaseRecoil) return;
        rb = deployer.SkillData.owner.GetComponent<Rigidbody2D>();
        RecoilDirection tempDir = deployer.SkillData.releaseRecoilDirection;
        if (tempDir == RecoilDirection.Up || tempDir == RecoilDirection.Down)
        {
            if (deployer.SkillData.key == ReflectKey.jump)
            {
                PlayerStatus.Instance.CanDoubleJump = false;
            }
            int direction = tempDir == RecoilDirection.Up ? 1 : -1;
            MonoMgr.Instance.StartCoroutine(UDMove(direction, deployer.SkillData.releaseRecoilSpeed, deployer.SkillData.releaseRecoilTime));
        }
        else if (tempDir == RecoilDirection.Left || tempDir == RecoilDirection.Right)
        {
            int direction = tempDir == RecoilDirection.Left ? -1 : 1;
            int face = PlayerStatus.Instance.IsFacingRight ? 1 : -1;
            if (PlayerStatus.Instance.IsWallSliding)
            {
                PlayerStatus.Instance.IsWallSliding = false;
                PlayerStatus.Instance.IsTouchingWall = false;
                deployer.SkillData.owner.GetComponent<CharacterMovement>().Flip();
                face *= -1;
            }
            MonoMgr.Instance.StartCoroutine(LRMove(direction, face, deployer.SkillData.releaseRecoilSpeed, deployer.SkillData.releaseRecoilTime));
        }

    }

    IEnumerator LRMove(int direction,int face,float speed,float time)
    {
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.EnableGravity = false;
        PlayerStatus.Instance.CanFlip = false;
        rb.velocity = new Vector2(speed * direction * face, 0);
        if (time != 0)
        {
            yield return new WaitForSeconds(time);
            rb.velocity = Vector2.zero;
            PlayerStatus.Instance.InputEnable = true;
            PlayerStatus.Instance.EnableGravity = true;
            PlayerStatus.Instance.CanFlip = true;
        }
        else
            yield return null;
    }

    IEnumerator UDMove(int direction,float speed,float time)
    {
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.EnableGravity = false;
        PlayerStatus.Instance.CanFlip = false;
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x, speed * direction);
        if (time != 0)
        {
            yield return new WaitForSeconds(time);
            rb.velocity = Vector2.zero;
            PlayerStatus.Instance.InputEnable = true;
            PlayerStatus.Instance.EnableGravity = true;
            PlayerStatus.Instance.CanFlip = true;
        }
        else
            yield return null;
    }
}
