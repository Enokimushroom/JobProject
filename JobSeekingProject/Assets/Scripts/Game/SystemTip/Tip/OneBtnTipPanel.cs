using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OneBtnTipPanel : BasePanel
{
    public event UnityAction btnNoHandler;

    public override void ShowMe()
    {
        transform.GetComponent<RectTransform>().localPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(880, 400);
        GetControl<Button>("btnNo").Select();
    }

    public void InitInfo(string info,UnityAction btnNo)
    {
        GetControl<Text>("txtQuestion").text = info;
        btnNoHandler += btnNo;
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
