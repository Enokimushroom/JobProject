using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTipsPanel : BasePanel
{
    private void Start()
    {
        EventCenter.Instance.AddEventListener<object>("CurrentPosShop", InitInfo);
    }

    public void InitInfo(object info)
    {
        if(info is ShopCellInfo)
        {
            ShopCellInfo info1 = info as ShopCellInfo;
            Item temp = GameDataMgr.Instance.GetItemInfo(info1.itemInfo.id);
            GetControl<Text>("ItemName").text = temp.name;
            GetControl<Text>("ItemTips").text = info1.tips;
        }
        else if(info is ItemInfo)
        {
            ItemInfo info2 = info as ItemInfo;
            Item temp = GameDataMgr.Instance.GetItemInfo(info2.id);
            GetControl<Text>("ItemName").text = temp.name;
            GetControl<Text>("ItemTips").text = temp.desInfo;
        }
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<object>("CurrentPosShop", InitInfo);
    }
}
