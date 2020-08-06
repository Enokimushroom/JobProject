using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 谈话类目标
/// </summary>
[System.Serializable]
public class TalkObjective : Objective
{
    /// <summary>
    /// NPC的ID
    /// </summary>
    [SerializeField] private string talkerID;
    public string TalkerID { get { return talkerID; } }

    public void UpdateTalkStatus(string id)
    {
        if(id == TalkerID)
        {
            Debug.Log("CheckTalker");
            UpdateStatus();
        }
    }
}
