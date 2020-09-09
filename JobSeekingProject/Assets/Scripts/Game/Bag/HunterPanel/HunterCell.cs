using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HunterCell : BasePanel,IItemBase<ItemInfo>
{
    private ItemInfo itemInfo;
    /// <summary>
    /// 初始化道具格子信息
    /// </summary>
    public void InitInfo(ItemInfo info)
    {
        this.itemInfo = info;
        //读取道具表
        HunterItem itemData = GameDataMgr.Instance.GetHunterItemInfo(info.id);
        //根据表中数据来更新信息
        //更新图标
        GetControl<Image>("imgName").sprite = ResMgr.Instance.Load<Sprite>(itemData.icon);
        //更新名字
        GetControl<Text>("txtName").text = itemData.name;
    }

    public ItemInfo GetItemInfo()
    {
        return this.itemInfo;
    }
}
