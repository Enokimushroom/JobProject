using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneMapHintTxtTrigger : TriggerBase
{
    [SerializeField] private string hint;
    [SerializeField] private Transform flockPos;
    private bool hasHint = false;

    public override void Action()
    {
        if (GameDataMgr.Instance.playerInfo.FirstTime)
        {
            GameDataMgr.Instance.playerInfo.FirstTime = false;
            PlayerStatus.Instance.UpdateRespawnPos(transform.position, MapMgr.Instance.GetCurrentMapType(), MapMgr.Instance.GetCurrentMapID());
            GameDataMgr.Instance.SavePlayerInfo();
            PEManager.Instance.GetParticleEffectOneOff("Flock", flockPos, Vector3.zero, Vector3.one, Quaternion.Euler(-30, -90, 0));
            MusicMgr.Instance.PlaySound("OpeningSting", false);
            MusicMgr.Instance.PlaySound("WindCaveLoop", true);
        }
        if (!hasHint)
        {
            hasHint = true;
            UIMgr.Instance.MapHintTxt(hint);
        }
    }
}
