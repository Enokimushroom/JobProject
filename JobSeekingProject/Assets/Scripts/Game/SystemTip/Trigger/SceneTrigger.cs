using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : TriggerBase
{
    [SerializeField] private MapType type;
    [SerializeField] private int mapID;
    [SerializeField] private string levelID;

    public override void Action()
    {
        MapMgr.Instance.SetMapInfo(type, mapID);
        LevelManager.Instance.SetCurrentLevelID(levelID);
    }
}
