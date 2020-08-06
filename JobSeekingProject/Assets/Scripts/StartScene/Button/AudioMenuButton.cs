using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenuButton : UIButton
{
    [SerializeField] AudioPanel menuButtonController;
    [SerializeField] AudioPanel.AudioMenuButton audioMenuButton;

    private void Update()
    {
        if (menuButtonController.index == (int)audioMenuButton)
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
        Debug.Log(menuButtonController.index);
        menuButtonController.index = (int)audioMenuButton;
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
