using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : UIButton
{
    [SerializeField] private MainStartPanel menuButtonController;
    [SerializeField] private MainStartPanel.MainMenuButton mainMenuButton;

    private void Update()
    {
        if (menuButtonController.index == (int)mainMenuButton)
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
        menuButtonController.index = (int)mainMenuButton;
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
