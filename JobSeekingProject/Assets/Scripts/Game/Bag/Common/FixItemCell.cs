using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FixItemCell : BasePanel
{
    [SerializeField] private int id;
    private ItemInfo itemInfo;

    /// <summary>
    /// 根据道具信息初始化格子信息
    /// </summary>
    public void InitInfo(ItemInfo info)
    {
        if (info != null)
        {
            //根据道具信息的数据，来更新格子对象
            Item itemData = GameDataMgr.Instance.GetItemInfo(info.id);
            //更新图标
            GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>(itemData.icon);
            if (itemData.type == 3)
                GetControl<Text>("txtNum").text = info.num.ToString();
        }
    }

    public void CheckFixItem()
    {
        if(GameDataMgr.Instance.playerInfo.fixItem.Any((x => x.id == id)))
        {
            itemInfo = new ItemInfo() { id = this.id, num = 1 };
            Item itemData = GameDataMgr.Instance.GetItemInfo(id);
            //更新图标
            GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>(itemData.icon);
            if (this.transform.name == "Money")
                GetControl<Text>("txtMoney").text = GameDataMgr.Instance.playerInfo.Money.ToString();
        }
        else
        {
            GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>("blank");
        }
    }

    public ItemInfo GetItemInfo()
    {
        return this.itemInfo;
    }

    public ItemInfo SetItemInfo(ItemInfo info)
    {
        this.itemInfo = info;
        return this.itemInfo;
    }
}
