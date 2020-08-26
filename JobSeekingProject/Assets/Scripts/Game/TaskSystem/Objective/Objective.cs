using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务目标
/// </summary>
public delegate void UpdateNextObjListener(Objective nextObj);
[System.Serializable]
public abstract class Objective
{
    /// <summary>
    /// 记录任务目标ID
    /// </summary>
    [HideInInspector] public string obID;

    /// <summary>
    /// 目标数目
    /// </summary>
    [SerializeField] private int amount;
    public int Amount { get { return amount; } }

    /// <summary>
    /// 现在数目
    /// </summary>
    [SerializeField] private int currentAmount;
    public int CurrentAmount
    {
        get { return currentAmount; }
        set
        {
            bool finish = IsFinish;
            if (value <= amount && value >= 0)
                currentAmount = value;
            else if (value < 0)
                currentAmount = 0;
            else
                currentAmount = amount;
            ///如果之前未完成，但这次完成了
            if (!finish && IsFinish && NextObjective != null)
                OnFinishThisEvent(NextObjective);
        }
    }

    /// <summary>
    /// 目标是否已完成
    /// </summary>
    public bool IsFinish
    {
        get
        {
            if (currentAmount >= amount)
                return true;
            return false;
        }
    }

    /// <summary>
    /// 目标是否按顺序排序
    /// </summary>
    [SerializeField] private bool inOrder;
    public bool InOrder { get { return inOrder; } }

    /// <summary>
    /// 如果是，排序数字（越小越前）
    /// </summary>
    [ConditionalHide("inOrder", true)]
    [SerializeField] private int orderIndex;
    public int OrderIndex { get { return orderIndex; } }

    /// <summary>
    /// 记录前后任务
    /// </summary>
    [System.NonSerialized] public Objective PreObjective;
    [System.NonSerialized] public Objective NextObjective;

    public event UpdateNextObjListener OnFinishThisEvent;

    public void UpdateStatus()
    {
        if (IsFinish) return;
        if (!InOrder) CurrentAmount++;
        else if (InOrder && AllPreObjFinish) CurrentAmount++;
    }

    /// <summary>
    /// 判定所有前置目标是否都完成
    /// </summary>
    protected bool AllPreObjFinish
    {
        get
        {
            Objective tempObj = PreObjective;
            while (tempObj != null)
            {
                if (!tempObj.IsFinish && tempObj.OrderIndex < OrderIndex)
                    return false;
                tempObj = tempObj.PreObjective;
            }
            return true;
        }
    }
}
