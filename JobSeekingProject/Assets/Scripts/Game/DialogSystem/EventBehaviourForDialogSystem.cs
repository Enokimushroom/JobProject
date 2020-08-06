using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Event", menuName = "Dialogue/Event")]
public class EventBehaviourForDialogSystem : ScriptableObject
{
    public void OpenShopBuyPanel()
    {
        Debug.Log("打开买商店");
        UIMgr.Instance.ShowPanel<BasePanel>("ShopBuyPanel", E_UI_Layer.Mid);
    }

    public void OpenShopSellPanel()
    {
        Debug.Log("打开卖商店");
        int temp = 0;
        foreach(ItemInfo info in GameDataMgr.Instance.playerInfo.numItem)
        {
            if (info.num == 0)
                temp++;
        }
        //证明背包没东西卖了
        if(temp == GameDataMgr.Instance.playerInfo.numItem.Count)
        {
            DialogBase db = ResMgr.Instance.Load<DialogBase>("ShopKeeperNo");
            DialogMgr.Instance.EnqueueDialog(db);
        }
        else
            UIMgr.Instance.ShowPanel<BasePanel>("ShopSellPanel", E_UI_Layer.Mid);
    }

    public void ShowNextDialog(DialogBase nextDB)
    {
        if (nextDB != null)
            DialogMgr.Instance.EnqueueDialog(nextDB);
    }
}
