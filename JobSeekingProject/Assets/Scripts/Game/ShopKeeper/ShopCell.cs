using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : BasePanel,IItemBase<ShopCellInfo>
{
    private ShopCellInfo info;
    private bool canBuy;

    private void OnEnable()
    {
        InitInfo(info);
    }

    /// <summary>
    /// 初始化商店物品信息
    /// </summary>
    public void InitInfo(ShopCellInfo info)
    {
        this.info = info;
        if (info != null)
        {
            Item item = GameDataMgr.Instance.GetItemInfo(info.itemInfo.id);
            //图标
            GetControl<Image>("CellImg").sprite = ResMgr.Instance.Load<Sprite>(item.icon);
            //价格
            GetControl<Text>("txtMoney").text = info.price.ToString();
            //判断身上的钱
            if (info.price > GameDataMgr.Instance.playerInfo.Money)
            {
                GetControl<Text>("txtMoney").color = new Color32(125, 125, 125, 255);
                canBuy = false;
            }
            else
            {
                GetControl<Text>("txtMoney").color = new Color32(255, 255, 255, 255);
                canBuy = true;
            }
        }
    }

    public void BuyItem()
    {
        //将物品添加给玩家,买完之后要在shopList里面消减
        GameDataMgr.Instance.BuyShop(info);
        //购买成功减少货币
        //GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.金钱, -info.price);
        MoneyDetails md = new MoneyDetails();
        md.moneySource = MoneyDetails.Source.ItemCell;
        md.moneyAmount = -info.price;
        PlayerStatus.Instance.ChangeMoney(md);
        //上句自带保存和分发事件
        Debug.Log("购买成功");
    }

    public ShopCellInfo GetShopCellInfo()
    {
        return info;
    }

    public bool ifCanBuy()
    {
        return canBuy;
    }
}
