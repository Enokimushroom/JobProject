using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanelBadge : BasePanel
{
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<ItemInfo>("CurrentPosBadge", InitInfo);
    }

    public void InitInfo(ItemInfo info)
    {
        if (info != null)
        {
            Item itemData = GameDataMgr.Instance.GetItemInfo(info.id);
            //如果是护符
            GetControl<Text>("nameBadge").text = itemData.name;
            GetControl<Text>("costBadge").text = "花费";
            //花费改为切图
            GetControl<Image>("imgBadge").sprite = ResMgr.Instance.Load<Sprite>(itemData.icon);
            GetControl<Text>("infoBadge").text = itemData.desInfo;
        }
        else
        {
            GetControl<Text>("nameBadge").text = null;
            GetControl<Text>("costBadge").text = null;
            GetControl<Image>("imgBadge").sprite = ResMgr.Instance.Load<Sprite>("blank");
            GetControl<Text>("infoBadge").text = null;
        }
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<ItemInfo>("CurrentPosBadge", InitInfo);
    }
}
