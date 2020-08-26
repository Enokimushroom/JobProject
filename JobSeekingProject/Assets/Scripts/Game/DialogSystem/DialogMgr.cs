using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMgr : UnityBaseManager<DialogMgr>
{
    /// <summary>
    /// 对话背景面板
    /// </summary>
    private DialogPanel dialogBox;
    //可额外添加项目
    //public Text dialogName;
    //public Text dialogText;
    //public Image dialogPortrait;

    /// <summary>
    /// 是否对话状态
    /// </summary>
    public bool inDialog { get; set; }

    /// <summary>
    /// 打字速度
    /// </summary>
    private float typeSpeed = 0.01f;
    /// <summary>
    /// 是否打字状态
    /// </summary>
    private bool isCurrentlyTyping;
    /// <summary>
    /// 打完字的整句内容
    /// </summary>
    private string completeText;
    /// <summary>
    /// 缓冲器
    /// </summary>
    private bool buffer;

    /// <summary>
    /// 对话队列
    /// </summary>
    private Queue<DialogBase.Info> dialogInfo = new Queue<DialogBase.Info>();
    /// <summary>
    /// 对话是否带有选项
    /// </summary>
    private bool isDialogOption;
    /// <summary>
    /// 对话选项个数
    /// </summary>
    private int optionsAmount;
    /// <summary>
    /// 对话选项UI
    /// </summary>
    private DialogOptionPanel dialogOptionUI;
    /// <summary>
    /// 选项按钮GO
    /// </summary>
    private GameObject[] optionButtons;

    /// <summary>
    /// 当前传入的说话对象
    /// </summary>
    public TaskGiver tg { get; set; }

    public delegate void Reward();
    public event Reward rewardEvent;

    public void EnqueueDialog(DialogBase db, TaskGiver giver = null)
    {
        if (inDialog) return;
        buffer = true;
        inDialog = true;
        PlayerStatus.Instance.IsForzen = true;
        GameManager.Instance.playerGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //读取说话人并触发说话事件
        if (giver != null)
        {
            tg = giver;
            tg.OnTalkBegin();
        }
        //缓冲器
        StartCoroutine(BufferTimer());
        //判断是否携带选项
        OptionsParser(db);
        //展现对话panel
        UIMgr.Instance.ShowPanel<BasePanel>("DialogPanel", E_UI_Layer.top,(obj)=> {
            dialogBox = obj.GetComponent<DialogPanel>();
        });
        //清空队列并逐句读取
        dialogInfo.Clear();
        foreach(DialogBase.Info info in db.dialogInfo)
        {
            dialogInfo.Enqueue(info);
        }
        //读取完成后输出第一行
        Invoke("DequeueDialog", 0.1f);
    }

    public void DequeueDialog()
    {
        //判断是否打字状态
        if (isCurrentlyTyping)
        {
            if (buffer) return;
            CompleteText();
            StopAllCoroutines();
            isCurrentlyTyping = false;
            return;
        }
        //判断是否已读取完成
        if (dialogInfo.Count == 0)
        {
            EndofDialog();
            if (tg != null)
                tg.OnTalkFinish();
            return;
        }
        //读取
        DialogBase.Info info = dialogInfo.Dequeue();
        completeText = info.contentTxt;
        //可额外添加内容
        //dialogName.text = info.speakerName;
        //dialogText.text = info.contentTxt;
        //dialogPortrait.sprite = info.protrait;
        dialogBox.GetDialogText().text = info.contentTxt;

        StartCoroutine(TypeText(info));
    }

    IEnumerator TypeText(DialogBase.Info info)
    {
        isCurrentlyTyping = true;
        dialogBox.GetDialogText().text = "";
        foreach (char c in info.contentTxt.ToCharArray())
        {
            yield return new WaitForSeconds(typeSpeed);
            dialogBox.GetDialogText().text += c;
        }
        isCurrentlyTyping = false;
    }

    /// <summary>
    /// 为了防止相同的触发按键而设置的缓冲器
    /// </summary>
    /// <returns></returns>
    IEnumerator BufferTimer()
    {
        yield return new WaitForSeconds(0.1f);
        buffer = false;
    }

    private void CompleteText()
    {
        dialogBox.GetDialogText().text = completeText;
    }

    public void EndofDialog()
    {
        //UIMgr.Instance.HidePanel("DialogPanel");
        UIMgr.Instance.PopPanel();
        OptionLogic();
    }

    private void OptionLogic()
    {
        if (isDialogOption)
        {
            dialogOptionUI.gameObject.SetActive(true);
            optionButtons[0].GetComponent<Button>().Select();
        }
        else
        {
            inDialog = false;
            rewardEvent?.Invoke();
            PlayerStatus.Instance.IsForzen = false;
        }

    }

    public void CloseOptions()
    {
        //dialogOptionUI.SetActive(false);
        //UIMgr.Instance.HidePanel("DialogOptionPanel");
        UIMgr.Instance.PopPanel();
    }

    public void OptionsParser(DialogBase db)
    {
        //判断当前对话是否携带选项
        if (db is DialogOptions)
        {
            isDialogOption = true;
            DialogOptions dialogOptions = db as DialogOptions;
            optionsAmount = dialogOptions.optionsInfo.Length;

            UIMgr.Instance.ShowPanel<BasePanel>("DialogOptionPanel", E_UI_Layer.top, (obj) => {
                obj.gameObject.SetActive(false);
                dialogOptionUI = obj.GetComponent<DialogOptionPanel>();
                optionButtons = dialogOptionUI.GetDialogButton();
                dialogOptionUI.GetDialogQuestionText().text = dialogOptions.questionText;

                for (int i = 0; i < optionButtons.Length; i++)
                {
                    optionButtons[i].SetActive(false);
                }

                for (int i = 0; i < optionsAmount; ++i)
                {
                    optionButtons[i].SetActive(true);
                    optionButtons[i].GetComponentInChildren<Text>().text = dialogOptions.optionsInfo[i].buttonName;
                    UnityEventHandlerForDialog myEventHandler = optionButtons[i].GetComponent<UnityEventHandlerForDialog>();
                    myEventHandler.eventHandler = dialogOptions.optionsInfo[i].myEvent;
                    //if (dialogOptions.optionsInfo[i].nextDialog != null)
                    //{
                    //    myEventHandler.myDialog = dialogOptions.optionsInfo[i].nextDialog;
                    //}
                    //else
                    //    myEventHandler.myDialog = null;
                }
            });

        }
        else
        {
            isDialogOption = false;
        }
    }
}
