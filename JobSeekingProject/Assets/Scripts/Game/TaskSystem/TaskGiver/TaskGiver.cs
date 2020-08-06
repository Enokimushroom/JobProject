using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGiver:NPC,ITalkAble
{
    [SerializeField]
    private Task[] tasksStore;
    public Task[] TasksStore { get { return tasksStore; } }

    [SerializeField, ReadOnly]
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
        if (ts == null) return;
        if (taskInstances.Count > 0) taskInstances.Clear();
        foreach(Task t in ts)
        {
            if (t)
            {
                Task tempT = Instantiate(t);
                foreach (CollectObjective co in tempT.CollectObjectives)
                    tempT.Objectives.Add(co);
                foreach (KillObjective ko in tempT.KillObjectives)
                    tempT.Objectives.Add(ko);
                foreach (TalkObjective to in tempT.TalkObjectives)
                    tempT.Objectives.Add(to);

                if (tempT.CmpltObjectiveInOrder)
                {
                    tempT.Objectives.Sort((x, y) =>
                    {
                        if (x.OrderIndex > y.OrderIndex) return 1;
                        else if (x.OrderIndex < y.OrderIndex) return -1;
                        else return 0;
                    });
                    for(int i = 1; i < tempT.Objectives.Count; ++i)
                    {
                        if (tempT.Objectives[i].OrderIndex >= tempT.Objectives[i - 1].OrderIndex)
                        {
                            tempT.Objectives[i].PreObjective = tempT.Objectives[i - 1];
                            tempT.Objectives[i - 1].NextObjective = tempT.Objectives[i];
                        }
                    }
                }
                int n1, n2, n3;
                n1 = n2 = n3 = 1;
                foreach(Objective o in tempT.Objectives)
                {
                    if(o is CollectObjective)
                    {
                        o.obID = tempT.TaskID + "_C0" + n1;
                        n1++;
                    }
                    if(o is KillObjective)
                    {
                        o.obID = tempT.TaskID + "_K0" + n2;
                        n2++;
                    }
                    if(o is TalkObjective)
                    {
                        o.obID = tempT.TaskID + "_T0" + n3;
                        n3 ++;
                    }
                }
                tempT.originTaskGiver = this;
                tempT.currentTaskGiver = this;
                TaskInstances.Add(tempT);
            }
        }
        currentTask = GetCurrentTask();
    }

    public Task GetCurrentTask()
    {
        foreach(Task task in TaskInstances)
        {
            if (!TaskMgr.Instance.HasCmpltTask(task) && task.AcceptAble)
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
        t.currentTaskGiver.TaskInstances.Remove(t);
        t.currentTaskGiver = this;
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
