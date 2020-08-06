using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来触发隐藏对话
/// </summary>
public class DialogTarget : Interactable
{
    public DialogTrigger DT;

    public override void Interact()
    {
        DT.index = 1;
    }

    public override void TooFar()
    {

    }
}
