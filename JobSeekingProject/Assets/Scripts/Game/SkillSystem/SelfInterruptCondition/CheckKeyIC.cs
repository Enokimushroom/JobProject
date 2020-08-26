using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKeyIC : IInterruptCondition
{
    private bool interrupt = false;
    private bool firstTime = true;

    public bool Check(Deployer deployer)
    {
        if (firstTime)
        {
            EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
            firstTime = false;
        }
        return interrupt;
    }

    public void OnFinish(Deployer deployer)
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
        MusicMgr.Instance.PlaySound("SuperSprintAirBrakeAudio", false);
    }

    private void CheckKeyDown(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.SuperSprint.CurrentKey)
        {
            interrupt = true;
        }
    }

    
}
