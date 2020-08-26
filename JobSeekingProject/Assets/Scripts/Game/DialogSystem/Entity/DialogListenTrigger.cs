using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogListenTrigger : DialogTriggerBase 
{
    private bool dialogSp = false;
    [SerializeField] private DialogBase[] specialDB;
    private Animator anim;


    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        tg = GetComponent<TaskGiver>();
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
        anim.SetBool("Talk", DialogMgr.Instance.inDialog);
        base.EnterAction(collision);
    }

    public virtual void AnimLeft()
    {
        anim.SetBool("Left", true);
    }

    public virtual void AnimRight()
    {
        anim.SetBool("Left", false);
    }

    public override void CheckDialog()
    {
        //玩家身上有任务，检测对话NPC是否为交任务NPC或者接任务NPC。
        if (TaskMgr.Instance.OnGoingTask != null)
        {
            Task t = ResMgr.Instance.Load<Task>(TaskMgr.Instance.OnGoingTask.TaskID);
            if (t.CmpltOnOriginalNpc && t.originTaskGiver._ID == tg._ID)
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
            else if (!t.CmpltOnOriginalNpc && t.CmpltNpcID == tg._ID && TaskMgr.Instance.CompleteTask(t))
            {
                DialogMgr.Instance.EnqueueDialog(t.OnCmpltDialog, tg);
                tg.currentTask = tg.GetCurrentTask();
            }
            else if (!t.CmpltOnOriginalNpc && t.originTaskGiver._ID == tg._ID)
            {
                DialogMgr.Instance.EnqueueDialog(t.OnGoingDialog, tg);
            }
            else
            {
                if (dialogSp && !DialogMgr.Instance.inDialog)
                {
                    int tempIndex = -1;
                    if (tempIndex < specialDB.Length - 1)
                        tempIndex++;
                    DialogMgr.Instance.EnqueueDialog(specialDB[tempIndex], tg);
                    if (tempIndex == specialDB.Length - 1)
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
            //玩家身上没任务，NPC身上有待接任务,就接
            if (tg.currentTask != null)
            {
                DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnGetDialog, tg);
                TaskMgr.Instance.AcceptTask(tg.currentTask);
            }
            //没有待接任务就正常输出
            else
            {
                if (dialogSp && !DialogMgr.Instance.inDialog)
                {
                    int tempIndex = -1;
                    if (tempIndex < specialDB.Length - 1)
                        tempIndex++;
                    DialogMgr.Instance.EnqueueDialog(specialDB[tempIndex], tg);
                    if (tempIndex == specialDB.Length - 1)
                        dialogSp = false;
                }
                else
                {
                    base.CheckDialog();
                }
            }
        }
    }
}
