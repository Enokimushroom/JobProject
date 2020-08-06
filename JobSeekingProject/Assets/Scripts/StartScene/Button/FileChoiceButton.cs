using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileChoiceButton : UIButton
{
    [SerializeField] private FileChoicePanel menuButtonController;
    [SerializeField] private FileChoicePanel.FileChoiceMenuButton fileMenuButton;

    private void Update()
    {
        if (menuButtonController.index == (int)fileMenuButton)
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
        menuButtonController.index = (int)fileMenuButton;
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
