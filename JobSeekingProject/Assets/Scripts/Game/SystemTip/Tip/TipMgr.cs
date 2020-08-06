using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TipMgr : BaseManager<TipMgr>
{
    public void ShowTwoBtnTip(string info, UnityAction btnYes)
    {
        UIMgr.Instance.ShowPanel<TwoBtnTipPanel>("TwoBtnTipPanel", E_UI_Layer.system, (panel) =>
        {
            panel.InitInfo(info, btnYes);
        });
    }
}
