using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : UnityBaseManager<MapMgr>
{
    private MapType mapType;
    private int mapID;
    private string currentSceneName;
    private Vector2 rebornPos;
    private Dictionary<int, Vector2> tempRebornPosDic = new Dictionary<int, Vector2>();

    public void Init()
    {
        mapType = GameDataMgr.Instance.playerInfo.MapType;
        mapID = GameDataMgr.Instance.playerInfo.MapID;
        LoadMap();
    }

    /// <summary>
    /// 读档,过图或者死亡时调用，已包括角色生成
    /// </summary>
    public void LoadMap()
    {
        //读取存档中地图类型和地图ID，拼凑出来并加载对应的场景
        //sceneName = mapType + mapID
        currentSceneName = mapType.ToString() + mapID.ToString();
        ScenesMgr.Instance.LoadSceneAsyn(currentSceneName, () =>
        {
            PlayerStatus.Instance.Reset();
            LevelManager.Instance.EnqueueLevel();
            SkillMgr.Instance.Init();
            TaskGiverMgr.Instance.Init();
            rebornPos = PlayerStatus.Instance.IsAlive ? rebornPos : PlayerStatus.Instance.RespawnPos;
            GameManager.Instance.RespawnPlayer(rebornPos);
            UIMgr.Instance.ShowPanel<BasePanel>("MainPanel", E_UI_Layer.Bot);
            MusicMgr.Instance.PlayBGMusic(currentSceneName + "BG");
            //读取地图上假重生点的坐标并记录下来方便以后调用
            tempRebornPosDic.Clear();
            TempRebornPosTrigger[] pos = FindObjectsOfType<TempRebornPosTrigger>();
            if (pos.Length == 0) return;
            else
            {
                for (int i = 0; i < pos.Length; ++i)
                {
                    int temp = i;
                    pos[temp].SetIndex(temp);
                    tempRebornPosDic.Add(temp, pos[temp].transform.position);
                }
            }
        });
    }

    /// <summary>
    /// 提供给场景触发器设置接下来要加载的地图信息
    /// </summary>
    public void SetMapInfo(MapType type, int mapID,Vector2 pos)
    {
        this.mapType = type;
        this.mapID = mapID;
        rebornPos = pos;
    }

    /// <summary>
    /// 提供给外部查看具体位置
    /// </summary>
    public Vector2 GetTempRebornPos(int index)
    {
        return tempRebornPosDic[index];
    }

    public MapType GetCurrentMapType()
    {
        return mapType;
    }

    public int GetCurrentMapID()
    {
        return mapID;
    }

}

public enum MapType
{
    KingPass,
    Dirtmouth,
    Crossroad,
    Colosseum,
}
