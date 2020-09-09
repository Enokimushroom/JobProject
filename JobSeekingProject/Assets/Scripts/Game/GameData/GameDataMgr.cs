using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    /// <summary>
    /// 各种数据的存储字典
    /// </summary>
    private Dictionary<int, Item> itemInfos = new Dictionary<int, Item>();
    private Dictionary<int, HunterItem> hunterItemInfos = new Dictionary<int, HunterItem>();

    /// <summary>
    /// 商店数据
    /// </summary>
    public List<ShopCellInfo> shopInfos;
    /// <summary>
    /// 玩家数据
    /// </summary>
    public Player playerInfo;
    /// <summary>
    /// 金钱增加量
    /// </summary>
    public float moneyDelta { get; set; }
    /// <summary>
    /// 玩家信息存储路径
    /// </summary>全平台通用可读可写的文件夹，热更重要途径
    public string PlayerInfo_Url = Application.persistentDataPath + "/SaveData";

    private string currentSavePath;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init(string path)
    {
        currentSavePath = path;
        //加载Resources文件夹下的json文件，获取它的内容
        Items items = LoadInfo<Items>("ItemInfo");
        HunterItems hunterItems = LoadInfo<HunterItems>("HunterItemInfo");
        Shops shopsInfo = LoadInfo<Shops>("ShopInfo");

        //ID和Item信息存进字典为了以后索引
        for (int i = 0; i < items.info.Count; ++i)
        {
            if (!itemInfos.ContainsKey(items.info[i].id))
                itemInfos.Add(items.info[i].id, items.info[i]);
        }

        for (int i = 0; i < hunterItems.hunterInfo.Count; ++i)
        {
            if (!hunterItemInfos.ContainsKey(hunterItems.hunterInfo[i].id))
                hunterItemInfos.Add(hunterItems.hunterInfo[i].id, hunterItems.hunterInfo[i]);
        }

        //记录下加载解析出来的商店信息
        shopInfos = shopsInfo.info;

        //初始化角色信息
        if (File.Exists(currentSavePath))
        {
            //把字符串转成玩家的数据结构
            string data = File.ReadAllText(currentSavePath);
            playerInfo = JsonConvert.DeserializeObject<Player>(data);
        }
        else
        {
            //没有玩家数据就初始化一个默认数据
            playerInfo = new Player();
            playerInfo.Init();
            //然后存储它
            SavePlayerInfo();
            ScenesMgr.Instance.LoadScene("FirstGame", null);
            return;
        }
        Debug.Log("核心数据加载成功");
        //为角色状态管理器作为观察者订阅核心数据的更新
        PlayerStatus.Instance.Init();
        //自定义键盘初始化
        KeyCodeMgr.Instance.Init();
        //已完成和未完成任务的初始化
        TaskMgr.Instance.Init();
        //关卡加载器初始化
        LevelManager.Instance.Init();
        //初始化地图管理器，读取角色档案所在地图（场景）
        MapMgr.Instance.Init();
        //已完成和未完成任务的初始化（交给地图加载器调用）
        //生成真正的人物角色（交给地图加载器调用）
        //初始化任务者管理器，记录并转移中转任务（交给地图加载器调用）
        //通知各位订阅者更新
        playerInfo.Notify();
        InputMgr.Instance.StartOrEndCheck(true);
        Debug.Log("GDM流程结束");
    }
    /// <summary>
    /// 读取文档转换数据
    /// </summary>
    private T LoadInfo<T>(string name)
    {
        string info = ResMgr.Instance.Load<TextAsset>(name).text;
        T items = JsonUtility.FromJson<T>(info);
        return items;
    }
    /// <summary>
    /// 获得物品时
    /// </summary>
    public void GetItem(ItemInfo item)
    {
        playerInfo.AddItem(item);
        SavePlayerInfo();
    }
    /// <summary>
    /// 获取任务
    /// </summary>
    public void AcceptTask(Task task)
    {
        playerInfo.currentTaskList.Add(task.TaskID);
        SavePlayerInfo();
    }
    /// <summary>
    /// 完成任务
    /// </summary>
    public void CompleteTask(Task task)
    {
        playerInfo.taskDoneList.Add(task.TaskID);
        playerInfo.currentTaskList.Remove(playerInfo.currentTaskList.Find(x => x == task.TaskID));
        SavePlayerInfo();
    }
    /// <summary>
    /// 穿上或卸下护符
    /// </summary>
    public void ChangeBadgeState(ItemInfo info,bool equiped)
    {
        if (equiped)
            playerInfo.CutEquipItem(info);
        else
            playerInfo.AddItem(info, true);
        SavePlayerInfo();
    }
    /// <summary>
    /// 返回凹槽未使用数目
    /// </summary>
    public int GetGrooveUnusedCount()
    {
        return playerInfo.GroHeld - playerInfo.GroUsed;
    }
    /// <summary>
    /// 返回凹槽已使用数目
    /// </summary>
    public int GetGrooveUsedCount()
    {
        return playerInfo.GroUsed;
    }
    /// <summary>
    /// 更改属性
    /// </summary>
    /// <param name="name">  属性名  </param>
    /// <param name="index">  增幅  </param>
    public void ChangePlayerAttri(PlayerInfoType info, float index)
    {
        playerInfo.ChangeAttri(info, index);
        if(info ==PlayerInfoType.金钱)
            moneyDelta += index;
        SavePlayerInfo();
        playerInfo.Notify();
    }
    /// <summary>
    /// 提供给外部的添加/减少playerData的观察者的方法
    /// </summary>
    public void AttachPlayerData(IObserver ob)
    {
        playerInfo.Attach(ob);
    }
    public void DetachPlayerData(IObserver ob)
    {
        playerInfo.Detach(ob);
    }
    public void AllOBDetach()
    {
        playerInfo.DetachAllOB();
    }
    /// <summary>
    /// 保存玩家信息
    /// </summary>
    public void SavePlayerInfo()
    {
        string saveData = JsonConvert.SerializeObject(playerInfo);
        File.WriteAllBytes(currentSavePath, Encoding.UTF8.GetBytes(saveData));
    }
    /// <summary>
    /// 根据道具ID 得到道具的详细信息
    /// </summary>
    public Item GetItemInfo(int id)
    {
        if (itemInfos.ContainsKey(id))
        {
            return itemInfos[id];
        }
        return null;
    }
    /// <summary>
    /// 根据日志ID 得到道具的详细信息
    /// </summary>
    public HunterItem GetHunterItemInfo(int id)
    {
        if (hunterItemInfos.ContainsKey(id))
            return hunterItemInfos[id];
        return null;
    }
    
    /// <summary>
    /// 根据猎物ID 添加到猎人日志
    /// </summary>
    public void AddHunterItem(int id)
    {
        playerInfo.AddHunterItem(id);
        SavePlayerInfo();
    }
    /// <summary>
    /// 购买商店物品
    /// </summary>
    public void BuyShop(ShopCellInfo info)
    {
        playerInfo.AddItem(info.itemInfo);
        playerInfo.RemoveShopItem(info);
        SavePlayerInfo();
    }
    /// <summary>
    /// 贩卖物品
    /// </summary>
    public void SellShop(ItemInfo info)
    {
        playerInfo.RemoveSellItem(info);
        SavePlayerInfo();
    }
    /// <summary>
    /// 完成收集物品任务时调用，扣除对应数量的物品
    /// </summary>
    public void RemoveTaskCoItem(int id,int num)
    {
        playerInfo.RemoveTaskItem(id, num);
        SavePlayerInfo();
    }
    /// <summary>
    /// 更新玩家的重生坐标
    /// </summary>
    public void UpdateRespawnPos(Vector2 pos,MapType type,int id)
    {
        playerInfo.UpdateRespawnPos(pos, type, id);
        SavePlayerInfo();
    }

    /// <summary>
    /// 提供给外部调用的检查是否有此Item
    /// </summary>
    public bool CheckIfHadItem(ItemInfo info)
    {
        return playerInfo.CheckIfHadItem(info);
    }

    public void UpdatePlayTime(float time)
    {
        playerInfo.playTime += time;
        SavePlayerInfo();
    }

}

/// <summary>
///临时结构体，用来表示道具表信息的数据结构
/// </summary>
public class Items
{
    public List<Item> info;
}

public class HunterItems
{
    public List<HunterItem> hunterInfo;
}

/// <summary>
/// 道具的基础信息
/// </summary>
//表示此类可以被序列化
[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string icon;
    public int type;
    public int cost;
    public string desInfo;
    public string skillID;
}

[System.Serializable]
public class HunterItem
{
    public int id;
    public string name;
    public string icon;
    public string img;
    public string desInfo;
    public string lockInfo;
    public int lockCondi;
}

/// <summary>
/// 玩家拥有的道具基础信息
/// </summary>
[System.Serializable]
public class ItemInfo
{
    public int id;
    public int num;
}

/// <summary>
/// 观察者接口
/// </summary>
public interface IObserver
{
    void UpdateData(ISubject sub);
}

/// <summary>
/// 观察目标接口
/// </summary>
public interface ISubject
{
    void Attach(IObserver ob);

    void Detach(IObserver ob);

    void Notify();
}

