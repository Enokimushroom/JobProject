using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 打怪类目标
/// </summary>
[System.Serializable]
public class KillObjective : Objective
{
    /// <summary>
    /// 敌人的ID
    /// </summary>
    [SerializeField] private string enermyID;
    public string EnermyID { get { return enermyID; } }

    public void UpdateKillAmount(string deadEnermyID)
    {
        Debug.Log(enermyID);
        if (deadEnermyID == enermyID)
        {
            Debug.Log("CheckEnermyDeath");
            UpdateStatus();
        }
    }
}
