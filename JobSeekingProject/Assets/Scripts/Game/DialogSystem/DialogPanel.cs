using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : BasePanel
{
    public override void ShowMe()
    {
        base.ShowMe();
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckInputDown);
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 270);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 500);
    }

    private void CheckInputDown(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.Interact.CurrentKey)
        {
            if (DialogMgr.Instance.inDialog)
            {
                DialogMgr.Instance.DequeueDialog();
            }
        }
    }

    public Text GetDialogText()
    {
        return GetControl<Text>("Dialogtxt");
    }

    public override void HideMe()
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInputDown);
    }

    
}
