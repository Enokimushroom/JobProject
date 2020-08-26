using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyPanel : BasePanel
{
    public RectTransform content;

    private Mugen<ShopCellInfo, ShopCell> sv;

    private GameObject seleObj;

    private int seleIndex;

    public override void ShowMe()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 0);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);

        sv = new Mugen<ShopCellInfo, ShopCell>();
        sv.InitCotentAndSVH(content, 580);
        sv.InitItemSizeAndCol(300, 100, 20, 1);
        sv.InitItemResName("ShopCell");
        sv.InitInfos(GameDataMgr.Instance.playerInfo.shopList);

        sv.CheckShowOrHide();

        //选择框
        seleObj = GetControl<Image>("imgSele").gameObject;
        seleIndex = sv.seleIndex;

        //禁用人物移动
        PlayerStatus.Instance.IsForzen = true;

        //告知位置
        Invoke("CheckSeleObjPos", 0.1f);

        //添加选择框监听事件
        AddInputListener();
    }

    private void CheckInput(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                if (seleIndex == 0) return;
                content.anchoredPosition += new Vector2(0, -120);
                sv.CheckShowOrHide();
                seleIndex = sv.seleIndex;
                break;
            case KeyCode.S:
                if (seleIndex == GameDataMgr.Instance.playerInfo.shopList.Count - 1) return;
                content.anchoredPosition += new Vector2(0, 120);
                sv.CheckShowOrHide();
                seleIndex = sv.seleIndex;
                break;
            case KeyCode.Escape:
                //退出
                Debug.Log("quit");
                UIMgr.Instance.PopPanel();
                return;
            case KeyCode.Space:
                //购买
                Debug.Log("buy");
                ConfirmItem();
                return;
        }
        CheckSeleObjPos();
    }

    private void Refresh()
    {
        for (int i = sv.oldMinIndex; i <= sv.oldMaxIndex; ++i)
        {
            if (sv.nowShowItems.ContainsKey(i))
            {
                if (sv.nowShowItems[i] != null)
                    PoolMgr.Instance.BackObj("ShopCell", sv.nowShowItems[i]);
                sv.nowShowItems.Remove(i);
            }
        }
        sv.nowShowItems.Clear();
        sv.CheckShowOrHide();
    }

    private void CheckSeleObjPos()
    {
        seleObj.transform.SetParent(sv.nowShowItems[seleIndex].transform);
        seleObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //告知TipsPanel现在是什么物体
        EventCenter.Instance.EventTrigger<object>("CurrentPosShop", sv.nowShowItems[seleIndex].GetComponent<ShopCell>().GetShopCellInfo());
    }

    private void ConfirmItem()
    {
        ShopCell itemWTB = seleObj.GetComponentInParent<ShopCell>();
        if (itemWTB.ifCanBuy())
        {
            string itemName = GameDataMgr.Instance.GetItemInfo(itemWTB.GetShopCellInfo().itemInfo.id).name;
            //点击购买后，弹出确认提示面板
            UIMgr.Instance.ShowConfirmPanel("是否缺人购买" + itemName, ConfirmType.TwoBtn, () => { itemWTB.BuyItem(); });
        }
        else
        {
            //可以考虑弹出一个对话框
        }
    }
    

    public override void HideMe()
    {
        RemoveInputListener();
        PlayerStatus.Instance.IsForzen = false;
    }

    public override void OnPause()
    {
        RemoveInputListener();
        transform.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        transform.gameObject.SetActive(true);
        //无论是否买了都要刷新列表
        Refresh();
        Invoke("AddInputListener", 0.1f);
    }

    private void RemoveInputListener()
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
    }

    private void AddInputListener()
    {
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckInput);
    }

}
