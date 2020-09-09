using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGiver:NPC,ITalkAble
{
    [SerializeField]
    private Task[] tasksStore;
    public Task[] TasksStore { get { return tasksStore; } }

    [SerializeField]
    private List<Task> taskInstances = new List<Task>();
    public List<Task> TaskInstances { get { return taskInstances; } set { taskInstances = value; } }

    public Task currentTask { get; set; }

    public event NPCTalkListener OnTalkBeginEvent;
    public event NPCTalkListener OnTalkFinishEvent;

    public void Init()
    {
        InitTask(TasksStore);
    }

    private void InitTask(Task[] ts)
    {
        if (ts.Length == 0) return;
        if (taskInstances.Count > 0) taskInstances.Clear();
        foreach(Task t in ts)
        {
            if (t)
            {
                foreach (CollectObjective co in t.CollectObjectives)
                    t.Objectives.Add(co);
                foreach (KillObjective ko in t.KillObjectives)
                    t.Objectives.Add(ko);
                foreach (TalkObjective to in t.TalkObjectives)
                    t.Objectives.Add(to);

                if (t.CmpltObjectiveInOrder)
                {
                    t.Objectives.Sort((x, y) =>
                    {
                        if (x.OrderIndex > y.OrderIndex) return 1;
                        else if (x.OrderIndex < y.OrderIndex) return -1;
                        else return 0;
                    });
                    for(int i = 1; i < t.Objectives.Count; ++i)
                    {
                        if (t.Objectives[i].OrderIndex >= t.Objectives[i - 1].OrderIndex)
                        {
                            t.Objectives[i].PreObjective = t.Objectives[i - 1];
                            t.Objectives[i - 1].NextObjective = t.Objectives[i];
                        }
                    }
                }
                int n1, n2, n3;
                n1 = n2 = n3 = 1;
                foreach(Objective o in t.Objectives)
                {
                    if(o is CollectObjective)
                    {
                        o.obID = t.TaskID + "_C0" + n1;
                        n1++;
                    }
                    if(o is KillObjective)
                    {
                        o.obID = t.TaskID + "_K0" + n2;
                        n2++;
                    }
                    if(o is TalkObjective)
                    {
                        o.obID = t.TaskID + "_T0" + n3;
                        n3 ++;
                    }
                }
                if (TaskMgr.Instance.HasOngoingTask(t) && !t.CmpltOnOriginalNpc && t.AcceptAble)
                {
                    if (!TaskGiverMgr.Instance.GiverTransferStation.ContainsKey(t.CmpltNpcID))
                    {
                        TaskGiverMgr.Instance.GiverTransferStation.Add(t.CmpltNpcID, ResMgr.Instance.Load<Task>(t.TaskID));
                    }
                    TaskGiverMgr.Instance.GiverTransferStation[t.CmpltNpcID].originTaskGiver = this._ID;
                    TaskGiverMgr.Instance.GiverTransferStation[t.CmpltNpcID].currentTaskGiver = this._ID;
                }
                else
                {
                    t.originTaskGiver = this._ID;
                    t.currentTaskGiver = this._ID;
                    TaskInstances.Add(t);
                }
            }
        }
        currentTask = GetCurrentTask();
    }

    public Task GetCurrentTask()
    {
        foreach(Task task in TaskInstances)
        {
            if (!TaskMgr.Instance.HasCmpltTaskWithID(task.TaskID) && task.AcceptAble)
            {
                return task;
            }
        }
        return null;
    }

    /// <summary>
    /// 转移任务交接对象（用于接任务和交任务不在同一个NPC的情况）
    /// </summary>
    /// <param name="t"></param>
    public void TransferTaskToThis(Task t)
    {
        if (!t) return;
        TaskInstances.Add(t);
        t.currentTaskGiver = this._ID;
    }

    public void OnTalkBegin()
    {
        OnTalkBeginEvent?.Invoke(this._ID);
    }

    public void OnTalkFinish()
    {
        OnTalkFinishEvent?.Invoke(this._ID);
    }
}

public delegate void NPCTalkListener(string talkerID);
public interface ITalkAble
{
    event NPCTalkListener OnTalkBeginEvent;
    event NPCTalkListener OnTalkFinishEvent;
    void OnTalkBegin();
    void OnTalkFinish();
}
