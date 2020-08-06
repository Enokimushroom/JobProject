using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商店售卖物品信息的数据
/// </summary>
[System.Serializable]
public class ShopCellInfo
{
    public int id;
    public ItemInfo itemInfo;
    public int price;
    public string tips;
}

/// <summary>
/// 作为json读取的中间数据结构，用来装载json内容
/// </summary>
public class Shops
{
    public List<ShopCellInfo> info;
}


