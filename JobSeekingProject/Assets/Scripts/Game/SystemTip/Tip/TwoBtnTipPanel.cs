using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TwoBtnTipPanel : BasePanel
{
    public event UnityAction btnYesHandler;
    public event UnityAction btnNoHandler;

    public override void ShowMe()
    {
        transform.GetComponent<RectTransform>().localPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(880, 400);
        GetControl<Button>("btnYes").Select();
    }


    public void InitInfo(string info,UnityAction btnYes,UnityAction btnNo)
    {
        GetControl<Text>("txtQuestion").text = info;
        btnYesHandler += btnYes;
        btnNoHandler += btnNo;
    }


    public void btnYes()
    {
        MonoMgr.Instance.StartCoroutine(Buffer());
        btnYesHandler?.Invoke();
        UIMgr.Instance.PopPanel();
    }

    public void btnNo()
    {
        MonoMgr.Instance.StartCoroutine(Buffer());
        btnNoHandler?.Invoke();
        UIMgr.Instance.PopPanel();
    }

    private IEnumerator Buffer()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerStatus.Instance.InputEnable = true;
        PlayerStatus.Instance.IsForzen = false;
    }
}
