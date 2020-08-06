using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeItemHintPanel : BasePanel
{
    public ItemInfo item;

    public override void ShowMe()
    {
        if (item != null)
        {
            Item i = GameDataMgr.Instance.GetItemInfo(item.id);
            GetControl<Image>("BadgeImg").sprite = ResMgr.Instance.Load<Sprite>(i.icon);
            GetControl<Text>("BadgeTxt").text = i.name;
        }
        Invoke("ShowOff", 3);
    }

    private void ShowOff()
    {
        transform.GetComponent<Animator>().SetTrigger("Off");

    }

    public void PopPanel()
    {
        UIMgr.Instance.PopPanel();
    }
}
