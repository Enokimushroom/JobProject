using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : BasePanel
{
    private ItemInfo itemInfo;

    /// <summary>
    /// 根据道具信息初始化格子信息
    /// </summary>
    public void InitInfo(ItemInfo info)
    {
        if (info != null)
        {
            this.itemInfo = info;
            //根据道具信息的数据，来更新格子对象
            Item itemData = GameDataMgr.Instance.GetItemInfo(info.id);
            //使用道具表里面的数据
            //更新图标
            GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>(itemData.icon);
        }
        else
        {
            GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>("BadgeDef");
        }
    }

    public ItemInfo GetItemInfo()
    {
        return this.itemInfo;
    }


    public void ItemOn()
    {
        //降半透明度
        GetControl<Image>("imgIcon").color = new Color(1, 1, 1, 0.5f);
    }
    
    public void ItemOff()
    {
        //复原
        GetControl<Image>("imgIcon").color = new Color(1, 1, 1, 1);
    }

}
