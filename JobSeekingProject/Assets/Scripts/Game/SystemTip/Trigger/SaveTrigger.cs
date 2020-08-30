using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : TriggerBase
{
    [SerializeField] private GameObject hint;
    private bool checkingSave;

    public override void Action()
    {
        hint.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCodeMgr.Instance.Interact.CurrentKey) && !checkingSave && PlayerStatus.Instance.OnGround)
        {
            checkingSave = true;
            PEManager.Instance.GetParticleEffectOneOff("ResetPE", transform, Vector3.zero, Vector3.one, Quaternion.Euler(-90, 0, 0));
            MusicMgr.Instance.PlaySound("UIHint_Save", false);
            PlayerStatus.Instance.UpdateRespawnPos(transform.position, MapMgr.Instance.GetCurrentMapType(), MapMgr.Instance.GetCurrentMapID());
            PlayerStatus.Instance.ChangeAttri(PlayerInfoType.当前血量, 999);
            UIMgr.Instance.ShowConfirmPanel("保存数据已更新", ConfirmType.OneBtn, null, () => { checkingSave = true; });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hint.SetActive(false);
        }
    }
}
