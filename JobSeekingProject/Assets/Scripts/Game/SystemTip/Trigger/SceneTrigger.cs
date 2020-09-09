using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : TriggerBase
{
    [SerializeField] private MapType type;
    [SerializeField] private int mapID;
    public string levelID;
    [SerializeField] private Vector2 pos;

    public override void Action()
    {
        GameObject.Destroy(collision.gameObject);
        ScenesMgr.Instance.goingScene = true;
        MapMgr.Instance.SetMapInfo(type, mapID, pos);
        LevelManager.Instance.SetCurrentLevelID(levelID);
        MapMgr.Instance.LoadMap();
    }
}
