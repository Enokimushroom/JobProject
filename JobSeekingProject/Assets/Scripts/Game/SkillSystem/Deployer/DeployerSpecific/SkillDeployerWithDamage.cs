using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class SkillDeployerWithDamage : Deployer
{
    private AttackDetails ad;
    private Rigidbody2D rb;

    /// <summary>
    /// 释放效果和移动
    /// </summary>
    public override void DeploySkill()
    {
        //执行影响算法
        ImpactTargets();

        //判断远程近程用不同的接口实现不同的方法
        MoveMode();

        //改名区分技能
        gameObject.name = SkillData.skillName;

        //非播放完立刻销毁而是有条件
        CheckSelfInterruption();

        rb = SkillData.owner.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("IDamagable"))
        {
            StartCoroutine(RepeatDamage(other.gameObject));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.CompareTag("Tramp"))//以后再加个陷阱的Tag
        {
            RecoilRection(other.gameObject);
        }

    }

    private IEnumerator RepeatDamage(GameObject other)
    {
        float atkTime = 0;
        do
        {
            //伤害目标生命,如果只伤害一次，就是把atkInterval和durationTime相等
            OnceDamage(other);
            yield return new WaitForSeconds(SkillData.damageInterval);
            atkTime += SkillData.damageInterval;
        } while (atkTime < SkillData.duration);
    }

    private void OnceDamage(GameObject other)
    {
        float rate = (SkillData.attackType == SkillAttackType.Magic) ? PlayerStatus.Instance.MagicAtkRate : PlayerStatus.Instance.AtkRate;
        float realDamage = (SkillData.baseDamage + PlayerStatus.Instance.BaseATK) * rate;
        ad.damageAmount = realDamage;
        ad.position = SkillData.owner.transform.position;
        ad.type = SkillData.attackType;
        other.GetComponent<IDamagable>().Damage(ad);
        RecoilRection(other);
    }

    private void RecoilRection(GameObject other)
    {
        if (SkillData.hasRecoil && (other.CompareTag("Enermy") || other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.CompareTag("Tramp")))
        {
            if (SkillData.skillID == "S00001")
            {
                //上挥砍
                if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.CompareTag("Tramp"))
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(SkillData.owner.transform.position, Vector2.up, 5.0f, LayerMask.GetMask("Ground"));
                    PEManager.Instance.GetParticleObjectDuringTime("HitGroundOrTrampEffect", null, hit2D.point, Vector3.one, Quaternion.Euler(0, 0, 180), 0.5f);
                }
                StartCoroutine(HitRecoil(true, -1));
            }
            else if (SkillData.skillID == "S00002")
            {
                //下挥砍
                if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.CompareTag("Tramp"))
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(SkillData.owner.transform.position, Vector2.down, 5.0f, LayerMask.GetMask("Ground"));
                    PEManager.Instance.GetParticleObjectDuringTime("HitGroundOrTrampEffect", null, hit2D.point, Vector3.one, Quaternion.identity, 0.5f);
                }
                StartCoroutine(HitRecoil(true, 1));
            }
            else if (SkillData.skillID == "S000" || SkillData.skillID == "S0001")
            {
                //左右挥砍
                int direction = PlayerStatus.Instance.IsFacingRight ? -1 : 1;
                if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.CompareTag("Tramp"))
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(SkillData.owner.transform.position, Vector2.right * -direction, 5.0f, LayerMask.GetMask("Ground"));
                    PEManager.Instance.GetParticleObjectDuringTime("HitGroundOrTrampEffect", null, hit2D.point, Vector3.one, Quaternion.Euler(0, 0, 90 * -direction), 0.5f);
                }
                StartCoroutine(HitRecoil(false, direction));
            }
        }
    }

    IEnumerator HitRecoil(bool ud,int direction)
    {
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.EnableGravity = false;
        PlayerStatus.Instance.CanFlip = false;
        rb.velocity = Vector2.zero;
        //左右的后坐力会被护符减少
        if (ud)
            rb.velocity = new Vector2(rb.velocity.x, SkillData.hitRecoilSpeed * direction);
        else
            rb.velocity = new Vector2(SkillData.hitRecoilSpeed * PlayerStatus.Instance.RecoilForceRate * direction, rb.velocity.y);
        yield return new WaitForSeconds(SkillData.hitRecoilTime);
        rb.velocity = Vector2.zero;
        PlayerStatus.Instance.InputEnable = true;
        PlayerStatus.Instance.EnableGravity = true;
        PlayerStatus.Instance.CanFlip = true;
    }

}
