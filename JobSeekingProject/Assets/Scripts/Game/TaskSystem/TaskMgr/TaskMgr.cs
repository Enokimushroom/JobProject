using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskMgr : BaseManager<TaskMgr>
{
    public List<Task> DoneTaskList { get; set; } = new List<Task>();
    public List<Task> OnGoingTaskList { get; set; } = new List<Task>();

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        DoneTaskList = GameDataMgr.Instance.playerInfo.taskDoneList;
        OnGoingTaskList = GameDataMgr.Instance.playerInfo.currentTaskList;
    }

    #region 任务查询
    public bool HasOngoingTask(Task task)
    {
        return OnGoingTaskList.Contains(task);
    }

    public bool HasCmpltTaskWithID(string id)
    {
        return DoneTaskList.Exists(x => x.TaskID == id);
    }

    public bool HasCmpltTask(Task task)
    {
        return DoneTaskList.Contains(task);
    }
    #endregion

    /// <summary>
    /// 接受任务
    /// </summary>
    public bool AcceptTask(Task task)
    {
        if (!task) return false;
        if (HasOngoingTask(task)) return false;
        foreach (Objective o in task.Objectives)
        {
            if (o is CollectObjective)
            {
                CollectObjective co = o as CollectObjective;
                GameDataMgr.Instance.playerInfo.OnGetItemEvent += co.UpdateCollectAmountUp;
                //获取时是否检查一遍背包以检测任务是否已可完成
                if (co.CheckBagAtAccept)
                {
                    int currentNum = GameDataMgr.Instance.playerInfo.hideList.Any(x => x.id == co.ItemID) ? GameDataMgr.Instance.playerInfo.hideList.Find(x => x.id == co.ItemID).num : 0;
                    co.UpdateCollectAmountUp(co.ItemID, currentNum);
                }
            }
            else if (o is KillObjective)
            {
                KillObjective ko = o as KillObjective;
                //检验是否为目的enermy的任务交给关卡管理器（它管理怪物生成和记录死亡的）
                LevelManager.Instance.OnDeathEvent += ko.UpdateKillAmount;
            }
            else if(o is TalkObjective)
            {
                TalkObjective to = o as TalkObjective;
                //检验是否为目的NPC的任务交给NPC管理器（它管理NPC的读取）
                TaskGiverMgr.Instance.OnTalkFinishEvent += to.UpdateTalkStatus;
            }
            o.OnFinishThisEvent += UpdateCollectObjectives;
        }
        task.IsOngoing = true;
        OnGoingTaskList.Add(task);
        GameDataMgr.Instance.AcceptTask(task);
        //如果这个任务不是在原NPC处交任务的话
        if (!task.CmpltOnOriginalNpc)
        {
            //储存到中转站
            TaskGiverMgr.Instance.GiverTransferStation.Add(task.CmpltNpcID, task);
            //如果现在场景中就有这个NPC,把这个人物转交给那个NPC
            if (TaskGiverMgr.Instance.AllTaskGiverInCurrentScene.ContainsKey(task.CmpltNpcID))
            {
                TaskGiverMgr.Instance.AllTaskGiverInCurrentScene[task.CmpltNpcID].TransferTaskToThis(task);
            }
        }
        return true;
    }

    /// <summary>
    /// 完成任务
    /// </summary>
    public bool CompleteTask(Task task)
    {
        if (!task) return false;
        if (HasOngoingTask(task) && task.IsComplete)
        {
            task.IsOngoing = false;
            OnGoingTaskList.Remove(task);
            DoneTaskList.Add(task);
            GameDataMgr.Instance.CompleteTask(task);
            //如果该任务在中转站中，需要消除
            if (TaskGiverMgr.Instance.GiverTransferStation.ContainsKey(task.CmpltNpcID))
            {
                TaskGiverMgr.Instance.GiverTransferStation.Remove(task.CmpltNpcID);
            }
            foreach(Objective o in task.Objectives)
            {
                o.OnFinishThisEvent -= UpdateCollectObjectives;
                if(o is CollectObjective)
                {
                    CollectObjective co = o as CollectObjective;
                    GameDataMgr.Instance.playerInfo.OnGetItemEvent -= co.UpdateCollectAmountUp;
                }
                if(o is KillObjective)
                {
                    KillObjective ko = o as KillObjective;
                    LevelManager.Instance.OnDeathEvent -= ko.UpdateKillAmount;
                }
                if(o is TalkObjective)
                {
                    TalkObjective to = o as TalkObjective;
                    TaskGiverMgr.Instance.OnTalkFinishEvent -= to.UpdateTalkStatus;
                }
            }
            //奖励
            foreach (ItemInfo reward in task.TaskRewards)
            {
                GameDataMgr.Instance.GetItem(reward);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 更新任务目标
    /// </summary>
    private void UpdateCollectObjectives(Objective nextObj)
    {
        Objective tempObj = nextObj;
        CollectObjective co;
        while (tempObj != null)
        {
            if(tempObj is CollectObjective)
            {
                co = tempObj as CollectObjective;
                co.CurrentAmount = GameDataMgr.Instance.playerInfo.hideList.Find(x => x.id == co.ItemID).num;
            }
            tempObj = tempObj.NextObjective;
            co = null;
        }
    }
}
