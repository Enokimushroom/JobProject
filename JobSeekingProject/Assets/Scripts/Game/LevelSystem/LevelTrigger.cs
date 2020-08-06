using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : Interactable
{
    public int temp;

    public override void Interact()
    {
        LevelManager.Instance.EnqueueLevel(LevelManager.Instance.currentLvID);
    }

    public override void TooFar()
    {

    }
}
