using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampTrigger : TriggerBase
{
    private bool isTrigger;

    public override void Action()
    {
        if (!isTrigger)
        {
            isTrigger = true;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerStatus.Instance.ChangeCurrentHealth(-1);
            if (PlayerStatus.Instance.CurrentHealth <= 0 && PlayerStatus.Instance.IsAlive)
            {
                collision.GetComponent<CharacterMovement>().Dead();
                return;
            }
            collision.GetComponent<Animator>().SetTrigger("TrampHit");
            MusicMgr.Instance.PlaySound("PlayerInjured", false);
            PEManager.Instance.GetParticleEffectOneOff("DeadWavePE", null, collision.transform.position, Vector3.one, Quaternion.identity);
            isTrigger = false;
        }
    }


}
