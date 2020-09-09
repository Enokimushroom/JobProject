using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanelHunter : BasePanel
{
    WaitForEndOfFrame delay = new WaitForEndOfFrame();

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<ItemInfo>("CurrentPosHunterTip", InitInfo);
    }


    public void InitInfo(ItemInfo info)
    {
        if (info!=null)
        {
            HunterItem itemData = GameDataMgr.Instance.GetHunterItemInfo(info.id);
            StartCoroutine(SetNativeSize());
            GetControl<Image>("imgItem").sprite = ResMgr.Instance.Load<Sprite>(itemData.img);
            GetControl<Text>("txtName").text = itemData.name;
            GetControl<Text>("txtDes").text = itemData.desInfo;
            if(info.num >= itemData.lockCondi)
            {
                GetControl<Text>("txtLock").text = itemData.lockInfo;
            }
            else
            {
                GetControl<Text>("txtLock").text = "请再猎杀" + (itemData.lockCondi - info.num).ToString() + "只以完成解析。";
            }
        }
        else
        {
            GetControl<Image>("imgItem").sprite = ResMgr.Instance.Load<Sprite>("blank");
            GetControl<Text>("txtName").text = null;
            GetControl<Text>("txtDes").text = null;
            GetControl<Text>("txtLock").text = null;
        }
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<ItemInfo>("CurrentPosHunterTip", InitInfo);
    }

    private IEnumerator SetNativeSize()
    {
        yield return delay;
        GetControl<Image>("imgItem").SetNativeSize();
    }
}
