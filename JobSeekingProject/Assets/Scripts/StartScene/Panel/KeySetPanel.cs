using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class KeySetPanel : BasePanel
{
    public enum KeyMenuButton
    {
        Up,
        Down,
        Left,
        Right,
        Bag,
        Menu,
        Attack,
        Jump,
        Sprint,
        SuperSprint,
        Magic,
        Interact,
        Back,
    }

    [HideInInspector] public int index;
    [HideInInspector] public int maxIndex;
    [HideInInspector] public UIButton[] menuButtons;
    private Animator anim;

    public override void ShowMe()
    {
        anim = GetComponent<Animator>();
        List<CustomButton> temp = KeyCodeMgr.Instance.list;
        for (int i = 0; i < temp.Count; ++i)
        {
            temp[i].AttachButton(GetControl<Button>(temp[i].DefaultFuncName));
        }
        PlayerStatus.Instance.IsForzen = true; 
        menuButtons = GetComponentsInChildren<UIButton>();
        index = 0;
        maxIndex = menuButtons.Length;
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    private void CheckKeyDown(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.Up.CurrentKey)
        {
            if (index > 0)
                index--;
            else
                index = maxIndex;
        }
        else if (key == KeyCodeMgr.Instance.Down.CurrentKey)
        {
            if (index < maxIndex)
                index++;
            else
                index = 0;
        }
        else if (key == KeyCode.Space)
        {
            if (index == (int)KeyMenuButton.Back)
            {
                menuButtons[index].animator.SetTrigger("Pressed");
                ButtonPress();
            }
        }
    }

    public void ButtonPress()
    {
        if(index == (int)KeyMenuButton.Back)
        {
            StartCoroutine(BackButton());
        }
    }

    IEnumerator BackButton()
    {
        anim.Play("KeyPanelFadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.PopPanel();
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (KeyCodeMgr.Instance.isWaitingForKey && e.isKey)
        {
            if (e.keyCode != KeyCode.KeypadEnter && e.keyCode != KeyCode.None)
            {
                KeyCodeMgr.Instance.SetNewKey(e.keyCode);
                StartCoroutine(WaitUpdate());
            }
        }
    }

    IEnumerator WaitUpdate()
    {
        yield return new WaitForEndOfFrame();
    }

    private void OnDisable()
    {
        CustomButton temp = KeyCodeMgr.Instance.currentButton;
        if (KeyCodeMgr.Instance.isWaitingForKey)
        {
            temp.DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = temp.CurrentKey.ToString();
            KeyCodeMgr.Instance.isWaitingForKey = false;
        }
    }

    public override void HideMe()
    {
        CustomButton temp = KeyCodeMgr.Instance.currentButton;
        if (KeyCodeMgr.Instance.isWaitingForKey)
        {
            temp.DefaultButton.transform.Find("btnTxt").GetComponent<Text>().text = temp.CurrentKey.ToString();
            KeyCodeMgr.Instance.isWaitingForKey = false;
        }
        PlayerStatus.Instance.IsForzen = false;
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }
}
