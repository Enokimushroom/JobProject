using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellCell : BasePanel
{
    private ItemInfo info;

    private void OnEnable()
    {
        InitInfo(info);
    }

    /// <summary>
    /// 初始化商店物品信息
    /// </summary>
    public void InitInfo(ItemInfo info)
    {
        this.info = info;
        if (info != null)
        {
            Item item = GameDataMgr.Instance.GetItemInfo(info.id);
            //图标
            GetControl<Image>("CellImg").sprite = ResMgr.Instance.Load<Sprite>(item.icon);
            //价格
            GetControl<Text>("txtMoney").text = item.cost.ToString();
            //个数
            GetControl<Text>("txtNum").text = info.num.ToString();
        }
    }

    public void SellItem()
    {
        GameDataMgr.Instance.SellShop(info);
        int price = GameDataMgr.Instance.GetItemInfo(info.id).cost;
        //GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.金钱, price);
        MoneyDetails md = new MoneyDetails();
        md.moneyAmount = price;
        md.moneySource = MoneyDetails.Source.ItemCell;
        PlayerStatus.Instance.ChangeMoney(md);
        Debug.Log("贩卖成功");
    }

    public ItemInfo GetSellInfo()
    {
        return info;
    }
}
