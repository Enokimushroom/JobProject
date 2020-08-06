using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : Interactable
{
    public DialogBase[] DB;
    [HideInInspector]public int index;
    //设置多个对话文本为了看起来不那么重复
    public bool nextDialogOnInteract;
    public GameObject hintObj;
    private Animator anim;
    private TaskGiver tg;
    private bool spDialog;
    [SerializeField] private DialogBase[] specialDB;

    private void Start()
    {
        index = nextDialogOnInteract ? -1 : 0;
        anim = this.GetComponent<Animator>();
        tg = GetComponent<TaskGiver>();
    }

    public override void Interact()
    {
        hintObj.SetActive(!PlayerStatus.Instance.IsForzen);
        if (Input.GetKeyDown(KeyCodeMgr.Instance.Interact.CurrentKey) && !PlayerStatus.Instance.IsForzen && PlayerStatus.Instance.OnGround)
        {
            //人物转向，抬头动画还是放在charactermovement里面方便启动和取消
            CharacterMovement cm = GameManager.Instance.playerGO.GetComponent<CharacterMovement>();
            if ((transform.position.x - cm.transform.position.x > 0 && !PlayerStatus.Instance.IsFacingRight) ||
                (transform.position.x - cm.transform.position.x < 0 && PlayerStatus.Instance.IsFacingRight))
            {
                cm.Flip();
            }
            if (tg.currentTask != null)
            {
                //如果有任务但是还没接，输出对话后，激活任务
                if (!TaskMgr.Instance.HasOngoingTask(tg.currentTask))
                {
                    DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnGetDialog,tg);
                    TaskMgr.Instance.AcceptTask(tg.currentTask);
                }
                //有任务并且已经接了，输出对话后，检查任务
                else if (TaskMgr.Instance.HasOngoingTask(tg.currentTask))
                {
                    if (!TaskMgr.Instance.CompleteTask(tg.currentTask))
                    {
                        DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnGoingDialot,tg);
                    }
                    else
                    {
                        DialogMgr.Instance.EnqueueDialog(tg.currentTask.OnCmpltDialog,tg);
                        tg.currentTask = tg.GetCurrentTask();
                    }
                }
            }
            else
            {
                if (spDialog && !DialogMgr.Instance.inDialog)
                {
                    int tempIndex = -1;
                    if (tempIndex < specialDB.Length - 1)
                        tempIndex++;
                    DialogMgr.Instance.EnqueueDialog(specialDB[tempIndex]);
                    if (tempIndex == specialDB.Length - 1)
                        spDialog = false;
                }
                else
                {
                    if (nextDialogOnInteract && !DialogMgr.Instance.inDialog)
                    {
                        if (index < DB.Length - 1)
                        {
                            index++;
                        }
                    }
                    DialogMgr.Instance.EnqueueDialog(DB[index]);
                }
            }
        }
        anim.SetBool("Talking", DialogMgr.Instance.inDialog);
    }

    public override void TooFar()
    {
        hintObj.SetActive(false);
    }
}
