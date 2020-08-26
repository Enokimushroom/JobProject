using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 玩家属性
/// </summary>
public enum PlayerInfoType
{
    当前血量=1,
    最大血量=2,
    法力=3,
    最大法力=4,
    基础攻击力=5,
    法术攻击基础系数=6,
    法术消耗基础系数=7,
    后坐力基础系数=8,
    受击后退距离基础系数=9,
    受击后无敌时间基础系数=10,
    攻击间隔基础系数=11,
    攻击距离基础系数=12,
    移动速度=13,
    跳跃高度=14,
    金钱=15,
    凹槽拥有数=16,
    凹槽已用数=17,
    冲刺间隔基础系数=18,
    金钱基础系数=19,
    普通攻击基础系数=20,
    打击力基础系数=21,
    治愈速度基础系数=22,
    治愈量基础系数=23,
    速度基础系数=24,
    回蓝量基础系数=25,
    金币拾取范围=26,
    地图显示位置=27,
}

public delegate void ItemListener(int itemID, int amount);

public class Player : ISubject
{
    #region 属性
    /// <summary>
    /// 是否第一次开始游戏
    /// </summary>
    public bool FirstTime { get; set; }

    /// <summary>
    /// 读档或者死亡时的重生地点
    /// </summary>
    public float RespawnPosX { get; set; }
    public float RespawnPosY { get; set; }

    /// <summary>
    /// 记录存档地图
    /// </summary>
    public MapType MapType { get; set; }
    public int MapID { get; set; }

    /// <summary>
    /// 最大血量
    /// </summary>
    public int MaxHp { get; set; }

    /// <summary>
    /// 当前血量
    /// </summary>
    private int hp;
    public int HP
    {
        get { return hp; }
        set
        {
            if (value < 0)
                value = 0;
            if (value > MaxHp)
                value = MaxHp;
            hp = value;
        }
    }

    /// <summary>
    /// 最大法力
    /// </summary>
    public float MaxSp { get; set; }

    /// <summary>
    /// 法力
    /// </summary>
    public float SP { get; set; }

    /// <summary>
    /// 移动速度
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// 跳跃高度
    /// </summary>
    public float JumpForce { get; set; }

    /// <summary>
    /// 金钱
    /// </summary>
    public int Money { get; set; }

    /// <summary>
    /// 凹槽拥有数
    /// </summary>
    public int GroHeld { get; set; }

    /// <summary>
    /// 凹槽已用数
    /// </summary>
    public int GroUsed { get; set; }

    /// <summary>
    /// 基础攻击力
    /// </summary>
    public int BaseATK { get; set; }

    /// <summary>
    /// 法术攻击基础系数
    /// </summary>
    private float magicAtkBaseRate;
    public float MagicAtkBaseRate {
        get { return magicAtkBaseRate; }
        set
        {
            if (value > 1.5f)
                value = 1.5f;
            magicAtkBaseRate = value;
        }
    }

    /// <summary>
    /// 法术消耗基础系数
    /// </summary>
    public float MagicCostBaseRate { get; set; }

    /// <summary>
    /// 后坐力基础系数
    /// </summary>
    public float RecoilForceBaseRate { get; set; }

    /// <summary>
    /// 受击后退距离基础系数
    /// </summary>
    public float FallBackBaseRate { get; set; }

    /// <summary>
    /// 受击后无敌时间基础系数
    /// </summary>
    public float HitRecoverBaseRate { get; set; }

    /// <summary>
    /// 攻击间隔基础系数
    /// </summary>
    public float AttackIntervalBaseRate { get; set; }

    /// <summary>
    /// 攻击距离基础系数
    /// </summary>
    private float attackDistanceBaseRate;
    public float AttackDistanceBaseRate
    {
        get { return attackDistanceBaseRate; }
        set
        {
            if (value > 1.5f)
                value = 1.5f;
            attackDistanceBaseRate = value;
        }
    }

    /// <summary>
    /// 冲刺间隔基础系数
    /// </summary>
    public float SprintCDBaseRate { get; set; }

    /// <summary>
    /// 金钱基础系数
    /// </summary>
    public float MoneyBaseRate { get; set; }

    /// <summary>
    /// 普通攻击基础系数
    /// </summary>
    public float AtkBaseRate { get; set; }

    /// <summary>
    /// 打击力基础系数（怪物受击后退力）
    /// </summary>
    public float KnockBackBaseRate { get; set; }

    /// <summary>
    /// 治愈速度基础系数
    /// </summary>
    public float HealingSpeedBaseRate { get; set; }

    /// <summary>
    /// 治愈量基础系数
    /// </summary>
    public float HealingAmountBaseRate { get; set; }

    /// <summary>
    /// 速度基础系数
    /// </summary>
    public float SpeedBaseRate { get; set; }

    /// <summary>
    /// 回蓝量基础系数
    /// </summary>
    public float SPIncreaseBaseRate { get; set; }
    #endregion

    #region 物品
    /// <summary>
    /// 固有物品
    /// </summary>
    public List<ItemInfo> fixItem { get; set; }

    /// <summary>
    /// 技能物品
    /// </summary>
    public List<ItemInfo> skillItem { get; set; }

    /// <summary>
    /// 可出售物品
    /// </summary>
    public List<ItemInfo> numItem { get; set; }

    /// <summary>
    /// 已解锁徽章
    /// </summary>
    public List<ItemInfo> badges { get; set; }

    /// <summary>
    /// 已装备徽章
    /// </summary>
    public List<ItemInfo> equiped { get; set; }

    /// <summary>
    /// 猎人日志信息
    /// </summary>
    public List<ItemInfo> hunterList { get; set; }

    /// <summary>
    /// 可在商店购买的剩余物品列表
    /// </summary>
    public List<ShopCellInfo> shopList { get; set; }

    /// <summary>
    /// 专门放置一些不会在背包上显示的物品。比如钥匙，面具碎片
    /// </summary>
    public List<ItemInfo> hideList { get; set; }
    #endregion

    #region 任务
    /// <summary>
    /// 存放已完成的任务序号
    /// </summary>
    public List<string> taskDoneList { get; set; }

    /// <summary>
    /// 正在执行的任务序号
    /// </summary>
    public List<string> currentTaskList { get; set; }
    #endregion

    /// <summary>
    /// 观察者列表
    /// </summary>
    private List<IObserver> observers = new List<IObserver>();

    public event ItemListener OnGetItemEvent;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {
        FirstTime = true;
        RespawnPosX = -53.25f;
        RespawnPosY = 28.0f;
        MapType = 0;
        MapID = 1;
        MaxHp = 5;
        HP = 5;
        MaxSp = 100.0f;
        SP = 100.0f;
        BaseATK = 10;
        Money = 500;
        GroHeld = 2;
        GroUsed = 0;

        Speed = 9;
        JumpForce = 50;

        SprintCDBaseRate = 1;
        AtkBaseRate = 1;
        magicAtkBaseRate = 1;
        MagicCostBaseRate = 1;
        AttackIntervalBaseRate = 1;
        attackDistanceBaseRate = 1;
        RecoilForceBaseRate = 1;
        FallBackBaseRate = 1;
        KnockBackBaseRate = 1;
        HitRecoverBaseRate = 1;
        HealingSpeedBaseRate = 1;
        MoneyBaseRate = 1;
        SpeedBaseRate = 1;
        HealingAmountBaseRate = 1;
        SPIncreaseBaseRate = 1;

        //面具，容器，骨钉
        fixItem = new List<ItemInfo>() { new ItemInfo() { id = 1, num = 1 }, new ItemInfo { id = 2, num = 1 }, new ItemInfo { id = 3, num = 1 }, new ItemInfo { id = 4, num = 1 }, new ItemInfo { id = 7, num = 1 }, new ItemInfo { id = 9, num = 1 } };
        numItem = new List<ItemInfo>() { new ItemInfo() { id = 10, num = 1 }};
        skillItem = new List<ItemInfo>() { new ItemInfo() { id = 15, num = 1 }};
        equiped = new List<ItemInfo>() { new ItemInfo() { id = 60, num = 1 } };
        badges = new List<ItemInfo>() { new ItemInfo() { id = 21, num = 1 }, new ItemInfo() { id = 60, num = 1 } };

        hunterList = new List<ItemInfo>() { new ItemInfo() { id = 1, num = 1 }, new ItemInfo() { id = 2, num = 1 }, new ItemInfo() { id = 3, num = 1 }, new ItemInfo() { id = 4, num = 1 },
                                            new ItemInfo() { id = 5, num = 1 },new ItemInfo() { id = 6, num = 1 },new ItemInfo() { id = 7, num = 1 },new ItemInfo() { id = 8, num = 1 },
                                            new ItemInfo() { id = 9, num = 1 }, new ItemInfo() { id = 10, num = 1 },new ItemInfo() { id = 11, num = 1 },new ItemInfo() { id = 12, num = 1 },
                                            new ItemInfo() { id = 13, num = 1 },new ItemInfo() { id = 14, num = 1 },new ItemInfo() { id = 15, num = 1 },new ItemInfo() { id = 16, num = 1 },
                                            new ItemInfo() { id = 17, num = 1 }};
        shopList = GameDataMgr.Instance.shopInfos;
        hideList = new List<ItemInfo>() { };
        taskDoneList = new List<string>();
        currentTaskList = new List<string>();

        //通知各位订阅者更新
        Notify();
    }

    /// <summary>
    /// 属性更改
    /// </summary>
    public void ChangeAttri(PlayerInfoType info, float index)
    {
        switch (info)
        {
            case PlayerInfoType.当前血量:
                HP += (int)index;
                break;
            case PlayerInfoType.法力:
                SP += index;
                break;
            case PlayerInfoType.最大血量:
                MaxHp += (int)index;
                break;
            case PlayerInfoType.基础攻击力:
                BaseATK += (int)index;
                break;
            case PlayerInfoType.金钱:
                Money += (int)index;
                break;
            case PlayerInfoType.凹槽已用数:
                GroUsed += (int)index;
                break;
            case PlayerInfoType.凹槽拥有数:
                GroHeld += (int)index;
                break;
        }
    }

    /// <summary>
    /// 背包添加Item
    /// </summary>
    public void AddItem(ItemInfo info, bool equiped = false)
    {
        Item item = GameDataMgr.Instance.GetItemInfo(info.id);
        switch (item.type)
        {
            case 1:
                GetFixItem(info);
                break;
            case 2:
                GetBadgeItem(info, equiped);
                break;
            case 3:
                GetCommonItem(info);
                break;
            case 4:
                GetSkillItem(info);
                break;
            case 5:
                GetHideItem(info);
                break;
        }
    }

    /// <summary>
    /// 获取技能物品
    /// </summary>
    /// <param name="info"></param>
    private void GetSkillItem(ItemInfo info)
    {
        //添加到数据库中，物品唯一，无需判重
        skillItem.Add(info);
        //人物展示提示面板
        UIMgr.Instance.ShowPanel<BasePanel>("SkillItemHintPanel", E_UI_Layer.top,(o)=>
        {
            o.GetComponent<SkillItemHintPanel>().item = info;
        });
        //读取物品信息，如果技能ID不为空，添加到技能管理器中
        Item temp = GameDataMgr.Instance.GetItemInfo(info.id);
        SkillMgr.Instance.AddSkill(temp.skillID);
    }

    /// <summary>
    /// 获取固定物品
    /// </summary>
    /// <param name="info"></param>
    private void GetFixItem(ItemInfo info)
    {
        fixItem.Add(info);
        if (GameDataMgr.Instance.GetItemInfo(info.id).skillID != string.Empty)
        {
            
            UIMgr.Instance.ShowPanel<BasePanel>("SkillItemHintPanel", E_UI_Layer.top, (o) =>
            {
                o.GetComponent<SkillItemHintPanel>().item = info;
            });
        }
    }

    /// <summary>
    /// 获取护符
    /// </summary>
    private void GetBadgeItem(ItemInfo info, bool equiped)
    {
        if (equiped)
            this.equiped.Add(info);
        else
        {
            badges.Add(info);
            UIMgr.Instance.ShowPanel<BasePanel>("BadgeItemHintPanel", E_UI_Layer.top, (o) =>
            {
                o.GetComponent<BadgeItemHintPanel>().item = info;
            });
        }
    }

    /// <summary>
    /// 获取可售物品
    /// </summary>
    /// <param name="info"></param>
    private void GetCommonItem(ItemInfo info)
    {
        if (numItem.Any(x => x.id == info.id))
            numItem.Find(x => x.id == info.id).num++;
        else
            numItem.Add(info);
        UIMgr.Instance.CommonHint(info);
    }

    /// <summary>
    /// 获取不显示在背包的物品
    /// </summary>
    /// <param name="info"></param>
    private void GetHideItem(ItemInfo info)
    {
        //面具碎片和钥匙
        if (hideList.Any(x => x.id == info.id))
            hideList.Find(x => x.id == info.id).num++;
        else
            hideList.Add(info);
        if (info.id != 62)
        {
            UIMgr.Instance.CommonHint(info);
        }
        else
        {
            int num = GameDataMgr.Instance.playerInfo.hideList.Find(x => x.id == 62).num;
            UIMgr.Instance.ShowPanel<BasePanel>("MaskHintPanel", E_UI_Layer.top,(o)=> 
            {
                o.GetComponent<MaskHintPanel>().TurnOnAnim(num);
                if (num >= 4)
                {
                    GameDataMgr.Instance.playerInfo.hideList.Find(x => x.id == 62).num -= 4;
                    MaxHp += 1;
                    GameDataMgr.Instance.SavePlayerInfo();
                }
            });
        }
        //给任务用的检查委托
        OnGetItemEvent?.Invoke(info.id, 1);
    }

    /// <summary>
    /// 背包删除Item
    /// </summary>
    public void CutEquipItem(ItemInfo info)
    {
        equiped.Remove(equiped.Find(x => x.id == info.id));
    }

    /// <summary>
    /// 添加猎人日志
    /// </summary>
    public void AddHunterItem(int id)
    {
        if (hunterList.Exists(x => x.id == id))
        {
            if (hunterList.Find(x => x.id == id).num < GameDataMgr.Instance.GetHunterItemInfo(id).lockCondi)
                hunterList.Find(x => x.id == id).num++;
        }
        else
        {
            hunterList.Add(new ItemInfo { id = id, num = 1 });
        }
    }

    /// <summary>
    /// 减少商店物品
    /// </summary>
    public void RemoveShopItem(ShopCellInfo info)
    {
        if (info.itemInfo.num > 1)
        {
            info.itemInfo.num -= 1;
            Debug.Log(GameDataMgr.Instance.GetItemInfo(info.itemInfo.id).name + "还剩下" + info.itemInfo.num.ToString());
            return;
        }
        shopList.Remove(info);
    }

    /// <summary>
    /// 减少身上可销售物品
    /// </summary>
    public void RemoveSellItem(ItemInfo info)
    {
        numItem.Find(x => x.id == info.id).num--;
    }

    /// <summary>
    /// 减少对应的任务物品
    /// </summary>
    public void RemoveTaskItem(int id ,int num)
    {
        hideList.Find(x => x.id == id).num -= num;
    }
    public void UpdateRespawnPos(Vector2 pos,MapType mapType,int mapID)
    {
        RespawnPosX = pos.x;
        RespawnPosY = pos.y;
        this.MapType = mapType;
        this.MapID = mapID;
    }

    /// <summary>
    /// 被观察者
    /// </summary>
    public void Attach(IObserver ob)
    {
        if (!observers.Contains(ob))
            this.observers.Add(ob);
        else
            Debug.Log("观察者" + ob.ToString() + "已订阅观察");
    }
    public void Detach(IObserver ob)
    {
        if (observers.Contains(ob))
        {
            this.observers.Remove(ob);
            Debug.Log("观察者" + ob.ToString() + "已解除观察关系");
        }
        else
            Debug.Log("观察者" + ob.ToString() + "没订阅观察");
    }
    public void DetachAllOB()
    {
        observers.Clear();
    }
    public void Notify()
    {
        foreach (IObserver ob in observers)
        {
            ob.UpdateData(this);
        }
    }
}
