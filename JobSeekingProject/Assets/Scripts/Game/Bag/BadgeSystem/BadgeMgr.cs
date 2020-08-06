using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeMgr : BaseManager<BadgeMgr>
{
    public Dictionary<int, ItemInfo> badgeUnlocked { get; private set; } = new Dictionary<int, ItemInfo>();
    public Dictionary<int, ItemInfo> badgeEquiped { get; private set; } = new Dictionary<int, ItemInfo>();
    public bool EquipedJudge { get; private set; }
    public bool ListChanged { get; private set; }

    public List<string> deliverList { get; private set; } = new List<string>();

    /// <summary>
    /// 数据初始化
    /// </summary>
    public void Init()
    {
        //读取已解锁的护符
        List<ItemInfo> unlockedList = GameDataMgr.Instance.playerInfo.badges;
        for(int i = 0; i < unlockedList.Count; ++i)
        {
            int temp = unlockedList[i].id;
            if (!badgeUnlocked.ContainsKey(temp))
            {
                badgeUnlocked.Add(temp, unlockedList[i]);
            }
        }
        //读取已装备的护符
        badgeEquiped.Clear();
        List<ItemInfo> equipedList = GameDataMgr.Instance.playerInfo.equiped;
        for(int i = 0; i < equipedList.Count; ++i)
        {
            int temp = equipedList[i].id;
            if (!badgeEquiped.ContainsKey(temp))
            {
                badgeEquiped.Add(temp, equipedList[i]);
            }
        }
        PassBadgeSkillData();
    }

    /// <summary>
    /// 护符的穿戴或卸下
    /// </summary>
    /// <param name="info"></param>
    public void BadgeOnOrOff(ItemInfo info)
    {
        if (info == null) return;

        Item temp = GameDataMgr.Instance.GetItemInfo(info.id);

        //如果已装备字典里面有这个id，说明已经装上了是想卸下
        if (badgeEquiped.ContainsKey(info.id))
        {
            //移出字典并告诉数据管理器数据更改
            badgeEquiped.Remove(info.id);
            GameDataMgr.Instance.ChangeBadgeState(info, true);
            //并更改凹槽使用数量
            GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.凹槽已用数, -1 * temp.cost);
            //告知别的类发生了变化并且现在是卸载状态
            ListChanged = true;
            EquipedJudge = false;
        }
        else
        {
            if (temp.cost <= GameDataMgr.Instance.GetGrooveUnusedCount())
            {
                //证明可以装上
                badgeEquiped.Add(info.id, info);
                GameDataMgr.Instance.ChangeBadgeState(info, false);
                //并更改凹槽使用数量
                GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.凹槽已用数, GameDataMgr.Instance.GetItemInfo(info.id).cost);
                //告知别的类发生了变化并且它现在是装载状态
                ListChanged = true;
                EquipedJudge = true;
            }
            else
            {
                ListChanged = false;
                EquipedJudge = false;
                Debug.Log("凹槽数目不足");
            }
        }
    }

    /// <summary>
    /// 提供给外部查询护符是否处于穿戴状态的方法
    /// </summary>
    public bool GetEquipedState(ItemInfo info)
    {
        if (info != null)
            return (badgeEquiped.ContainsKey(info.id));
        return false;
    }

    /// <summary>
    /// 背包面板关闭时调用，整理已装备护符的信息给技能管理器调用（记录了护符所代表的技能ID List)
    /// </summary>
    public void PassBadgeSkillData()
    {
        deliverList.Clear();
        foreach (ItemInfo info in badgeEquiped.Values)
        {
            Item detail = GameDataMgr.Instance.GetItemInfo(info.id);
            if (!deliverList.Contains(detail.skillID) && detail.skillID != string.Empty)
                deliverList.Add(detail.skillID);
        }
        SkillMgr.Instance.GetLastestSkill(deliverList);
    }
}
