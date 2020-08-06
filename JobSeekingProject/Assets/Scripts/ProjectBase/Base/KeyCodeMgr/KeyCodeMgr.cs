using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeMgr : BaseManager<KeyCodeMgr>
{
    #region 按键
    public CustomButton Up { get; private set; }
    public CustomButton Down { get; private set; }
    public CustomButton Left { get; private set; }
    public CustomButton Right { get; private set; }
    public CustomButton Attack { get; private set; }
    public CustomButton Jump { get; private set; }
    public CustomButton Sprint { get; private set; }
    public CustomButton SuperSprint { get; private set; }
    public CustomButton Bag { get; private set; }
    public CustomButton Recover { get; private set; }
    public CustomButton Menu { get; private set; }
    public CustomButton Interact { get; private set; }
    #endregion

    /// <summary>
    /// 当前指向的键位
    /// </summary>
    public CustomButton currentButton { get; set; }

    /// <summary>
    /// 当前所有键位列表
    /// </summary>
    public List<CustomButton> list { get; set; } = new List<CustomButton>();

    /// <summary>
    /// 等待键盘输入状态
    /// </summary>
    public bool isWaitingForKey = false;

    /// <summary>
    /// 初次运行按键初始化
    /// </summary>
    public void Init()
    {
        Up = new CustomButton("up", KeyCode.W);
        Down = new CustomButton("down", KeyCode.S);
        Left = new CustomButton("left", KeyCode.A);
        Right = new CustomButton("right", KeyCode.D);
        Bag = new CustomButton("bag", KeyCode.B);
        Attack = new CustomButton("attack", KeyCode.J);
        Jump = new CustomButton("jump", KeyCode.Space);
        Sprint = new CustomButton("sprint", KeyCode.L);
        SuperSprint = new CustomButton("superSprint", KeyCode.K);
        Recover = new CustomButton("recover", KeyCode.O);
        Menu = new CustomButton("menu", KeyCode.Escape);
        Interact = new CustomButton("interact", KeyCode.E);
    }

    /// <summary>
    /// 设置新键
    /// </summary>
    public void SetNewKey(KeyCode keyCode)
    {
        currentButton.CurrentKey = keyCode;
        KeyConflictDetection();
        currentButton.DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = currentButton.CurrentKey.ToString();
        PlayerPrefs.SetString(currentButton.DefaultFuncName, currentButton.CurrentKey.ToString());
        isWaitingForKey = false;
    }

    /// <summary>
    /// 键位冲突检测
    /// </summary>
    private void KeyConflictDetection()
    {
        foreach (CustomButton temp in list)
        {
            if (temp != currentButton && temp.CurrentKey == currentButton.CurrentKey)
            {
                temp.CurrentKey = KeyCode.None;
                PlayerPrefs.SetString(temp.DefaultFuncName, temp.CurrentKey.ToString());
                temp.DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = "";
                break;
            }
        }
    }

    /// <summary>
    /// 恢复默认按键
    /// </summary>
    public void ResetBindKeys()
    {
        foreach (CustomButton temp in list)
        {
            temp.CurrentKey = temp.DefaultKey;
            temp.DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = temp.CurrentKey.ToString();
            PlayerPrefs.SetString(temp.DefaultFuncName, temp.CurrentKey.ToString());
        }
    }

}
