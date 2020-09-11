using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogListenTrigger : DialogTriggerBase
{
    private int spIndex;
    private bool dialogSp = false;
    [SerializeField] private bool hasAnim;
    private List<DialogBase> specialDB = new List<DialogBase>();
    private Animator anim;

    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        tg = GetComponent<TaskGiver>();
        spIndex = -1;
    }

    public override void EnterAction(Collider2D collision)
    {
        if (collision.transform.position.x < transform.position.x)
        {
            AnimLeft();
        }
        else
        {
            AnimRight();
        }
        if (hasAnim)
            anim.SetBool("Talk", DialogMgr.Instance.inDialog);
        base.EnterAction(collision);
    }

    public virtual void AnimLeft()
    {
        if(hasAnim)
            anim.SetBool("Left", true);
    }

    public virtual void AnimRight()
    {
        if (hasAnim)
            anim.SetBool("Left", false);
    }

    public override void CheckDialog()
    {
        //玩家身上有任务，检测对话NPC是否为交任务NPC或者接任务NPC。
        if (TaskMgr.Instance.OnGoingTask != null)
        {
            Task t = ResMgr.Instance.Load<Task>(TaskMgr.Instance.OnGoingTask.TaskID);
            if (t.CmpltOnOriginalNpc && t.originTaskGiver == tg._ID)
            {
                if (TaskMgr.Instance.CompleteTask(t))
                {
                    DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnCmpltDialog, tg);
                    tg.currentTask = tg.GetCurrentTask();
                }
                else
                {
                    DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnGoingDialog, tg);
                }
            }
            else if (!t.CmpltOnOriginalNpc && t.CmpltNpcID == tg._ID)
            {
                if (TaskMgr.Instance.CompleteTask(t))
                {
                    DialogMgr.Instance.EnqueueDialog(t.OnCmpltDialog, tg);
                    tg.currentTask = tg.GetCurrentTask();
                }
                else
                {
                    Objective currentOb = null;
                    for (int i = 0; i < t.Objectives.Count; ++i)
                    {
                        if (!t.Objectives[i].IsFinish)
                        {
                            currentOb = t.Objectives[i];
                            break;
                        }
                    }
                    if (currentOb is TalkObjective)
                    {
                        if ((currentOb as TalkObjective).TalkerID == tg._ID)
                        {
                            if (dialogSp && !DialogMgr.Instance.inDialog)
                            {
                                if (spIndex < specialDB.Count - 1)
                                    spIndex++;
                                DialogMgr.Instance.EnqueueDialog(specialDB[spIndex], tg);
                                if (spIndex == specialDB.Count - 1)
                                    dialogSp = false;
                            }
                        }
                    }
                    else
                    {
                        base.CheckDialog();
                    }
                }
            }
            else if (!t.CmpltOnOriginalNpc && t.originTaskGiver == tg._ID)
            {
                DialogMgr.Instance.EnqueueDialog(t.OnGoingDialog, tg);
            }
            else
            {
                if (dialogSp && !DialogMgr.Instance.inDialog)
                {
                    if (spIndex < specialDB.Count - 1)
                        spIndex++;
                    DialogMgr.Instance.EnqueueDialog(specialDB[spIndex], tg);
                    if (spIndex == specialDB.Count - 1)
                        dialogSp = false;
                }
                else
                {
                    base.CheckDialog();
                }
            }
        }
        else
        {
            //玩家身上没任务，NPC身上有待接且未完成任务,就接
            if (tg.GetCurrentTask() != null)
            {
                tg.currentTask = tg.GetCurrentTask();
                if (!TaskMgr.Instance.HasCmpltTaskWithID(tg.currentTask.TaskID))
                {
                    DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnGetDialog, tg);
                    TaskMgr.Instance.AcceptTask(tg.currentTask);
                }
            }
            //没有待接任务就正常输出
            else
            {
                if (dialogSp && !DialogMgr.Instance.inDialog)
                {
                    if (spIndex < specialDB.Count - 1)
                        spIndex++;
                    DialogMgr.Instance.EnqueueDialog(specialDB[spIndex], tg);
                    if (spIndex == specialDB.Count - 1)
                        dialogSp = false;
                }
                else
                {
                    base.CheckDialog();
                }
            }
        }
    }

    public void SetDialogSp(DialogBase db, bool push)
    {
        dialogSp = true;
        //如果是任务加载的特殊对话，需要设置spIndex
        //如果是物品加载的特殊对话，无需设置，让其完成任务后下次对话自动触发对话，index==sp.count，然后跳出特殊对话。
        if(push)
            spIndex = specialDB.Count;
        specialDB.Add(db);
    }
}
