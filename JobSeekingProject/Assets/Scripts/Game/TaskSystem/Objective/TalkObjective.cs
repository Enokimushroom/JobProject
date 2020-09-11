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
    
    /// <summary>
    /// 触发交谈时的内容
    /// </summary>
    [SerializeField] private DialogBase talkDb;
    public DialogBase TalkDb { get { return talkDb; } }

    public void UpdateTalkStatus(string id)
    {
        if(id == TalkerID)
        {
            Debug.Log("CheckTalker");
            UpdateStatus();
        }
    }
}
