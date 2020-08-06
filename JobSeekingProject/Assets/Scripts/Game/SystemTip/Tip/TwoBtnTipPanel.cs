using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TwoBtnTipPanel : BasePanel
{
    public event UnityAction btnYesHandler;

    public override void ShowMe()
    {
        transform.GetComponent<RectTransform>().localPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 500);
        GetControl<Button>("btnYes").Select();
    }


    public void InitInfo(string info,UnityAction btnYes)
    {
        GetControl<Text>("txtQuestion").text = info;
        btnYesHandler += btnYes;
    }


    public void btnYes()
    {
        btnYesHandler?.Invoke();
        UIMgr.Instance.PopPanel();
    }

    public void btnNo()
    {
        UIMgr.Instance.PopPanel();
    }
}
