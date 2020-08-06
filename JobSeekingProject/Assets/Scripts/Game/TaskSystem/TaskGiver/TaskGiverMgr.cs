﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGiverMgr : UnityBaseManager<TaskGiverMgr>
{
    /// <summary>
    /// 当前场景所有任务NPC
    /// </summary>
    public Dictionary<string, TaskGiver> AllTaskGiverInCurrentScene { get; set; } = new Dictionary<string, TaskGiver>();

    /// <summary>
    /// 任务转移中转站
    /// </summary>
    public Dictionary<string, Task> GiverTransferStation { get; set; } = new Dictionary<string, Task>();


    /// <summary>
    /// NPC交谈任务检测中转站
    /// </summary>
    /// <param name="talkerID"></param>
    public delegate void CheckTalkerStatus(string talkerID);
    public event CheckTalkerStatus OnTalkFinishEvent;

    /// <summary>
    /// 初始化地图上NPC的信息（每次地图加载完成后由地图管理器调用）
    /// </summary>
    public void Init()
    {
        AllTaskGiverInCurrentScene.Clear();
        TaskGiver[] taskGivers = FindObjectsOfType<TaskGiver>();
        foreach(TaskGiver giver in taskGivers)
        {
            AllTaskGiverInCurrentScene.Add(giver._ID, giver);
            giver.OnTalkFinishEvent += CheckTalker;
            if (GiverTransferStation.ContainsKey(giver._ID))
            {
                giver.TransferTaskToThis(GiverTransferStation[giver._ID]);
            }
        }
        foreach(KeyValuePair<string,TaskGiver> kvp in AllTaskGiverInCurrentScene)
        {
            kvp.Value.Init();
        }

    }

    public void CheckTalker(string talkerID)
    {
        OnTalkFinishEvent?.Invoke(talkerID);
    }

}
