using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CustomButton
{
    /// <summary>
    /// 默认功能名称(跟UI面板上Button的名字保持一致)
    /// </summary>
    public string DefaultFuncName { get; set; }

    /// <summary>
    /// 相联的UI Button对象
    /// </summary>
    public Button DefaultButton { get; set; }

    /// <summary>
    /// 默认按键
    /// </summary>
    public KeyCode DefaultKey { get; set; }

    /// <summary>
    /// 当前绑定按键
    /// </summary>
    public KeyCode CurrentKey { get; set; }

    /// <summary>
    /// 关联的技能ID
    /// </summary>
    public string SkillID { get; set; }

    public CustomButton(string funcName,KeyCode key)
    {
        DefaultFuncName = funcName;
        DefaultKey = key;

        CurrentKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(DefaultFuncName, DefaultKey.ToString()));
        KeyCodeMgr.Instance.list.Add(this);
    }

    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    private void BtnClick()
    {
        if (KeyCodeMgr.Instance.currentButton != this)
        {
            if (KeyCodeMgr.Instance.currentButton != null)
            {
                KeyCodeMgr.Instance.currentButton.DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = KeyCodeMgr.Instance.currentButton.CurrentKey.ToString();
            }
            KeyCodeMgr.Instance.currentButton = this;
        }
        DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = "";
        KeyCodeMgr.Instance.isWaitingForKey = true;
        Debug.Log("waitingforkey");
    }

    /// <summary>
    /// 关联对应的UI
    /// </summary>
    public void AttachButton(Button button)
    {
        DefaultButton = button;
        DefaultButton.onClick.RemoveAllListeners();
        DefaultButton.onClick.AddListener(BtnClick);
        DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = CurrentKey.ToString();
    }

    /// <summary>
    /// 关联对应技能ID
    /// </summary>
    public void AttachSkill(string id)
    {
        SkillID = id;
    }

}
