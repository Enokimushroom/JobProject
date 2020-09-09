using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TASK",menuName = "Task/New task")]
public class Task:ScriptableObject
{
    /// <summary>
    /// 任务ID
    /// </summary>
    [SerializeField] private string taskID;
    public string TaskID { get { return taskID; } }

    /// <summary>
    /// 任务NPC对话交互
    /// </summary>
    [SerializeField] private DialogBase onGetDialog;
    public DialogBase OnGetDialog { get { return onGetDialog; } }
    [SerializeField] private DialogBase onGoingDialog;
    public DialogBase OnGoingDialog { get { return onGoingDialog; } }
    [SerializeField] private DialogBase onCmpltDialog;
    public DialogBase OnCmpltDialog { get { return onCmpltDialog; } }

    /// <summary>
    /// 是否有任务顺序
    /// （如果有，则按照index从小到大依次进行，相同index代表可以同时进行，如果无，则代表不受顺序影响，都同时进行）
    /// </summary>
    [SerializeField] private bool cmpltObjectiveInOrder;
    public bool CmpltObjectiveInOrder { get { return cmpltObjectiveInOrder; } }
    /// <summary>
    /// 承接和完成任务的NPC是否是同一个人
    /// </summary>
    [Space]
    [SerializeField] private bool cmpltOnOriginalNpc = true;
    public bool CmpltOnOriginalNpc { get { return cmpltOnOriginalNpc; } }
    /// <summary>
    /// 完成任务的NPC编号
    /// </summary>
    [ConditionalHide("cmpltOnOriginalNpc", true, true)]
    [SerializeField] private string cmpltNpcID;
    public string CmpltNpcID { get { return cmpltNpcID; } }

    /// <summary>
    /// 任务触发条件（需要前置任务或者任务物品）
    /// </summary>
    [SerializeField] private TaskAcceptCondition[] acceptConditions;
    public TaskAcceptCondition[] AcceptConditions { get { return acceptConditions; } }

    public bool AcceptAble
    {
        get
        {
            foreach(TaskAcceptCondition tac in acceptConditions)
            {
                if (!tac.IsEligible) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 存储所有任务目标（分为收集，打怪，谈话）
    /// </summary>
    [Space]
    [System.NonSerialized] private List<Objective> objectives = new List<Objective>();
    public List<Objective> Objectives { get { return objectives; } }
    /// <summary>
    /// 收集
    /// </summary>
    [SerializeField] private CollectObjective[] collectObjectives;
    public CollectObjective[] CollectObjectives { get { return collectObjectives; } }
    /// <summary>
    /// 打怪
    /// </summary>
    [SerializeField] private KillObjective[] killObjectives;
    public KillObjective[] KillObjectives { get { return killObjectives; } }
    /// <summary>
    /// 谈话
    /// </summary>
    [SerializeField] private TalkObjective[] talkObjectives;
    public TalkObjective[] TalkObjectives { get { return talkObjectives; } }
    /// <summary>
    /// 任务的起源和现在的指向
    /// </summary>
    public string currentTaskGiver { get; set; }
    public string originTaskGiver { get; set; }
    /// <summary>
    /// 任务是否正在执行
    /// </summary>
    [HideInInspector] public bool IsOngoing;
    /// <summary>
    /// 任务是否完成
    /// </summary>
    public bool IsComplete
    {
        get
        {
            foreach (CollectObjective co in collectObjectives)
                if (!co.IsFinish) return false;
            foreach (KillObjective ko in killObjectives)
                if (!ko.IsFinish) return false;
            foreach (TalkObjective to in talkObjectives)
                if (!to.IsFinish) return false;
            return true;
        }
    }
    /// <summary>
    /// 奖励的物品
    /// </summary>
    [SerializeField] private ItemInfo[] taskRewards;
    public ItemInfo[] TaskRewards { get { return taskRewards; } }
}
