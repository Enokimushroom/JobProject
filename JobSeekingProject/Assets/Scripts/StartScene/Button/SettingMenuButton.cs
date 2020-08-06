using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuButton : UIButton
{
    [SerializeField] private SettingPanel menuButtonController;
    [SerializeField] private SettingPanel.SettingMenuButton settingMenuButton;

    private void Update()
    {
        if(menuButtonController.index == (int)settingMenuButton)
        {
            animator.SetBool("Selected", true);
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }

    public override void MousePointerEnter()
    {
        menuButtonController.index = (int)settingMenuButton;
        base.MousePointerEnter();
    }

    public override void MousePointerExit()
    {
        base.MousePointerExit();
    }

    public override void MousePointerClick()
    {
        base.MousePointerClick();
        menuButtonController.ButtonPress();
    }

}
