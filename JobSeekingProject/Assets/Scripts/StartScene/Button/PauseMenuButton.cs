using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : UIButton
{
    [SerializeField] private PausePanel pauseButtonController;
    [SerializeField] private PausePanel.PausePanelButton pauseMenuButton;

    private void Update()
    {
        if (pauseButtonController.index == (int)pauseMenuButton)
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
        pauseButtonController.index = (int)pauseMenuButton;
        base.MousePointerEnter();
    }

    public override void MousePointerExit()
    {
        base.MousePointerExit();
    }

    public override void MousePointerClick()
    {
        base.MousePointerClick();
        pauseButtonController.ButtonPress();
    }
}
