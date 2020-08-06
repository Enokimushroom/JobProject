using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanelInStartScene : BasePanel
{
    [HideInInspector]public int index;
    [HideInInspector] public int maxIndex;
    [HideInInspector] public UIButton[] menuButtons;

    public virtual void Start()
    {
        menuButtons = GetComponentsInChildren<UIButton>();
        index = 0;
        maxIndex = menuButtons.Length;
    }

    public virtual void CheckKeyDown(KeyCode key)
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
            menuButtons[index].animator.SetTrigger("Pressed");
            ButtonPress();
        }
    }

    public virtual void ButtonPress()
    {

    }

}
