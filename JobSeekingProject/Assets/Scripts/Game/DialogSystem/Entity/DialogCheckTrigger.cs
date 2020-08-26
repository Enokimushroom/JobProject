using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCheckTrigger : DialogTriggerBase
{
    private bool dialogSp = false;
    [SerializeField] private DialogBase[] specialDB;

    public override void CheckDialog()
    {
        if (dialogSp && !DialogMgr.Instance.inDialog)
        {
            int tempIndex = -1;
            if (tempIndex <= specialDB.Length - 1)
                tempIndex++;
            DialogMgr.Instance.EnqueueDialog(specialDB[tempIndex]);
            if (tempIndex == specialDB.Length - 1)
                dialogSp = false;
        }
        else
        {
            base.CheckDialog();
        }
    }

    public void DialogSpOn()
    {
        dialogSp = true;
    }
}
