using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanelCommon : BasePanel
{
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<ItemInfo>("CurrentPosCommon", InitInfo);
    }

    public void InitInfo(ItemInfo info)
    {
        if (info != null)
        {
            Item itemData = GameDataMgr.Instance.GetItemInfo(info.id);
            GetControl<Text>("nameItem").text = itemData.name;
            GetControl<Text>("infoItem").text = itemData.desInfo;
        }
        else
        {
            GetControl<Text>("nameItem").text = null;
            GetControl<Text>("infoItem").text = null;
        }
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<ItemInfo>("CurrentPosCommon", InitInfo);
    }
}
