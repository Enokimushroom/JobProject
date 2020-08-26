using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellPanel : BasePanel
{
    public RectTransform content;
    private GameObject seleObj;
    private Dictionary<int, GameObject> seleGrid = new Dictionary<int, GameObject>();
    private List<ItemInfo> shopSellList = new List<ItemInfo>();
    private int seleNum;

    public override void ShowMe()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 0);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);

        //创建格子
        CreateSellCell();

        //选择框
        seleObj = GetControl<Image>("imgSele").gameObject;

        //禁用人物移动
        PlayerStatus.Instance.IsForzen = true;

        //告知位置
        Invoke("CheckSeleObjPos", 0.1f);

        AddInputListener();
    }

    private void CheckInput(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                if (seleNum == 0) return;
                seleNum--;
                break;
            case KeyCode.S:
                if (seleNum == shopSellList.Count - 1) return;
                seleNum++;
                break;
            case KeyCode.Escape:
                //退出
                Debug.Log("quit");
                UIMgr.Instance.PopPanel();
                return;
            case KeyCode.Space:
                //购买
                Debug.Log("sell");
                ConfirmItem();
                return;
        }
        CheckSeleObjPos();
    }

    private void CreateSellCell()
    {
        foreach(ItemInfo info in GameDataMgr.Instance.playerInfo.numItem)
        {
            if (info.num != 0)
                shopSellList.Add(info);
        }

        if(shopSellList.Count == 0)
        {
            UIMgr.Instance.PopPanel();
            DialogBase db = ResMgr.Instance.Load<DialogBase>("ShopKeeperNo");
            DialogMgr.Instance.EnqueueDialog(db);
        }
        else
        {
            for (int i = 0; i < shopSellList.Count; ++i)
            {
                int temp = i;
                seleGrid.Add(temp, null);
                PoolMgr.Instance.GetObj("ShopSellCell", (obj) =>
                {
                    obj.transform.SetParent(content);
                    obj.transform.localScale = Vector2.one;
                    obj.transform.localPosition = new Vector3(150, -(temp + 1) * 120, 0);
                    obj.GetComponent<ShopSellCell>().InitInfo(shopSellList[temp]);

                    if (seleGrid.ContainsKey(temp))
                        seleGrid[temp] = obj;
                    else
                        PoolMgr.Instance.BackObj("ShopSellCell", obj);
                });
            }
        }
        

    }

    private void Refresh()
    {
        for(int i = 0; i < shopSellList.Count; ++i)
        {
            if (seleGrid.ContainsKey(i))
            {
                if (seleGrid[i] != null)
                    PoolMgr.Instance.BackObj("ShopSellCell", seleGrid[i]);
                seleGrid.Remove(i);
            }
        }
        seleGrid.Clear();
        shopSellList.Clear();
        CreateSellCell();
        if(shopSellList.Count!=0)
            CheckSeleObjPos();
    }

    private void ConfirmItem()
    {
        ShopSellCell itemWTB = seleObj.GetComponentInParent<ShopSellCell>();
        string itemName = GameDataMgr.Instance.GetItemInfo(itemWTB.GetSellInfo().id).name;
        UIMgr.Instance.ShowConfirmPanel("是否确认出售" + itemName, ConfirmType.TwoBtn, () => { itemWTB.SellItem(); });
    }

    private void CheckSeleObjPos()
    {
        if (seleNum >= shopSellList.Count - 1)
            seleNum = shopSellList.Count - 1;
        seleObj.transform.SetParent(seleGrid[seleNum].transform);
        seleObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        EventCenter.Instance.EventTrigger<object>("CurrentPosShop", seleObj.GetComponentInParent<ShopSellCell>().GetSellInfo());
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
