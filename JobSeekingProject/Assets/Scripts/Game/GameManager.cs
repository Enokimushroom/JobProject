using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 
public class GameManager : UnityBaseManager<GameManager>
{
    [HideInInspector] public GameObject playerGO;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //InputMgr.Instance.StartOrEndCheck(true);
        //KeyCodeMgr.Instance.Init();
        //UIMgr.Instance.ShowPanel<BasePanel>("MainStartPanel", E_UI_Layer.Bot);
        //核心数据初始化（内涵各个管理器初始化和角色生成）
        GameDataMgr.Instance.Init(GameDataMgr.Instance.PlayerInfo_Url+ "PlayerInfo.txt");
        //显示主面板
        UIMgr.Instance.ShowPanel<BasePanel>("MainPanel", E_UI_Layer.Bot);
    }

    public void RespawnPlayer()
    {
        ResMgr.Instance.LoadAsync<GameObject>("Player", (obj) =>
        {
            obj.transform.position = PlayerStatus.Instance.respawnPos;
            playerGO = obj;
            PlayerStatus.Instance.IsAlive = true;
            PlayerStatus.Instance.InputEnable = true;
            //固定特殊技能的初始化
            SkillMgr.Instance.Init();
            //护符及护符技能的初始化
            BadgeMgr.Instance.Init();
        });
    }

    public void ClearWhenChangeScene()
    {
        MusicMgr.Instance.MusicClear();
        PoolMgr.Instance.PoolClear();
        EventCenter.Instance.EventTriggerClear();
    }
    
}
