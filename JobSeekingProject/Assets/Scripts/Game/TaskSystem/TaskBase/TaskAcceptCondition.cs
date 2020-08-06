using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskCondition
{
    None = 1,
    ComplexTask = 2,
    HasItem = 4,
}

/// <summary>
/// 任务接受条件
/// </summary>
[System.Serializable]
public class TaskAcceptCondition
{
    /// <summary>
    /// 条件类型
    /// </summary>
    [SerializeField] private TaskCondition acceptCondition;
    public TaskCondition AcceptCondition { get { return acceptCondition; } }

    /// <summary>
    /// 前置任务ID（如果是这个类型）
    /// </summary>
    [ConditionalHide("acceptCondition", (int)TaskCondition.ComplexTask, true)]
    [SerializeField] private string cmpltTaskID;
    public string CmpltTaskID { get { return cmpltTaskID; } }

    /// <summary>
    /// 前置任务（如果是这个类型）
    /// </summary>
    [ConditionalHide("acceptCondition", (int)TaskCondition.ComplexTask, true)]
    [SerializeField] private Task cmpltTask;
    public Task CmpltTask { get { return cmpltTask; } }

    /// <summary>
    /// 触发任务的物品ID（如果是这个类型）
    /// </summary>
    [ConditionalHide("acceptCondition", (int)TaskCondition.HasItem, true)]
    [SerializeField] private int ownedItemID;
    public int OwnedItemID { get { return ownedItemID; } }

    /// <summary>
    /// 触发任务的物品（如果是这个类型）
    /// </summary>
    [ConditionalHide("acceptCondition", (int)TaskCondition.HasItem, true)]
    [SerializeField] private ItemInfo ownedItem;
    public ItemInfo OwnedItem { get { return ownedItem; } }

    /// <summary>
    /// 判断前置条件是否满足
    /// </summary>
    public bool IsEligible
    {
        get
        {
            switch (AcceptCondition)
            {
                case TaskCondition.ComplexTask:
                    if (CmpltTaskID != string.Empty)
                        return TaskMgr.Instance.HasCmpltTaskWithID(CmpltTaskID);
                    else return TaskMgr.Instance.HasCmpltTaskWithID(CmpltTask.TaskID);
                case TaskCondition.HasItem:
                    if (OwnedItemID != 0)
                        return GameDataMgr.Instance.playerInfo.hideList.Contains(OwnedItem);
                    else return GameDataMgr.Instance.playerInfo.hideList.Exists(x => x.id == OwnedItem.id);
                default: return false;
            }
        }
    }
}


