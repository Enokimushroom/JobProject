using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMenuButton : UIButton
{
    [SerializeField]private KeySetPanel menuButtonController;
    [SerializeField]private KeySetPanel.KeyMenuButton keyMenuButton;

    private void Update()
    {
        if (menuButtonController.index == (int)keyMenuButton)
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
        menuButtonController.index = (int)keyMenuButton;
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
