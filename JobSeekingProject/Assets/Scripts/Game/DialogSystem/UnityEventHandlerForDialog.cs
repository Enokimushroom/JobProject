using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventHandlerForDialog : MonoBehaviour
{
    [HideInInspector]public UnityEvent eventHandler;

    public void InitiateAction()
    {
        StartCoroutine(InDialogBuffer());
    }

    IEnumerator InDialogBuffer()
    {
        yield return new WaitForSeconds(0.01f);
        DialogMgr.Instance.CloseOptions();
        DialogMgr.Instance.inDialog = false;

        eventHandler?.Invoke();

        //把后续对话作为eventHandler中的一种
        //if (myDialog != null)
        //    DialogMgr.Instance.EnqueueDialog(myDialog);
    }
}
