using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 收集类目标
/// </summary>
[System.Serializable]
public class CollectObjective : Objective
{
    /// <summary>
    /// 需要收集的物品ID
    /// </summary>
    [SerializeField] private int itemID;
    public int ItemID { get { return itemID; } }

    /// <summary>
    /// 标识是否在接任务时检查背包道具是否已满足任务目标，否则目标重新计算数量
    /// </summary>
    [SerializeField] private bool checkBagAtAccept = true;
    public bool CheckBagAtAccept
    {
        get { return checkBagAtAccept; }
        set { checkBagAtAccept = value; }
    }

    /// <summary>
    /// 更新获取的道具数量（用于被背包系统的拾取事件订阅）
    /// </summary>
    /// <param name="itemID">  背包系统拾取的物品ID </param>
    /// <param name="leftAmount"> 拾取数量 </param>
    public void UpdateCollectAmountUp(int itemID, int leftAmount = 1)
    {
        if (GameDataMgr.Instance.GetItemInfo(itemID).type != 5) return;
        if (itemID == this.ItemID)
        {
            Debug.Log("CheckItem");
            for (int i = 0; i < leftAmount; ++i)
            {
                UpdateStatus();
            }
        }
    }
}
