using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGrid : MonoBehaviour
{
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("InitInfo", InitInfo);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("InitInfo", InitInfo);
    }

    void InitInfo()
    {
        List<ItemInfo> temp = GameDataMgr.Instance.playerInfo.skillItem;
        for(int i = 0; i < temp.Count; ++i)
        {
            FixItemCell cell = ResMgr.Instance.Load<GameObject>("FixItemCell").GetComponent<FixItemCell>();
            cell.transform.SetParent(this.transform);
            cell.InitInfo(temp[i]);
            cell.SetItemInfo(temp[i]);
        }
    }
}
