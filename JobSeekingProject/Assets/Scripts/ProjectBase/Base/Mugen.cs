using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemBase<T>
{
    void InitInfo(T info);
}

/// <summary>
/// 无限物品
/// </summary>
public class Mugen<T,K> where K:IItemBase<T>
{
    private RectTransform content;
    //可视范围高
    private int viewPortH;

    //当前显示的格子对象
    public Dictionary<int, GameObject> nowShowItems = new Dictionary<int, GameObject>();

    //数据来源
    private List<T> items;

    //记录上一次显示的索引范围
    public int oldMinIndex = -1;
    public int oldMaxIndex = -1;

    //格子的间隔宽高
    private int itemW;
    private int itemH;
    private int itemSpace;

    //格子的列数
    private int col;

    //格子预设体名字
    private string itemResName;
    //当前选中格子序号
    public int seleIndex;

    /// <summary>
    /// 初始化conten和可视范围高
    /// </summary>
    public void InitCotentAndSVH(RectTransform trans,int h)
    {
        this.content = trans;
        this.viewPortH = h;
    }

    /// <summary>
    /// 初始化数据来源和content的高
    /// </summary>
    public void InitInfos(List<T> items)
    {
        this.items = items;
        content.sizeDelta = new Vector2(0, Mathf.CeilToInt(items.Count / col + 4) * (itemH + itemSpace));
    }

    /// <summary>
    /// 初始化格子间隔大小和列数
    /// </summary>
    public void InitItemSizeAndCol(int w,int h,int space,int col)
    {
        this.itemW = w;
        this.itemH = h;
        this.itemSpace = space;
        this.col = col;
    }

    /// <summary>
    /// 初始化格子资源
    /// </summary>
    public void InitItemResName(string name)
    {
        itemResName = name;
    }

    public void CheckShowOrHide()
    {
        //如果滑动小于0了，不处理
        if (content.anchoredPosition.y < 0) return;

        //检查哪些格子可以被显示出来
        int minIndex = ((int)(content.anchoredPosition.y / (itemH + itemSpace)) - 2) * col;
        int maxIndex = ((int)((content.anchoredPosition.y + viewPortH) / (itemH + itemSpace)) - 2) * col + col - 1;

        seleIndex = (minIndex + maxIndex) / 2 ;

        if (seleIndex >= items.Count)
            seleIndex = items.Count;

        if (minIndex < 0)
            minIndex = 0;

        //索引不能超出道具最大数量-1
        if (maxIndex >= items.Count)
            maxIndex = items.Count - 1;

        if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
        {
            //在记录当前索引之前
            //根据上一次索引和这一次新算的索引来判断哪些该删除
            //删除上一节溢出部分
            for (int i = oldMinIndex; i < minIndex; ++i)
            {
                if (nowShowItems.ContainsKey(i))
                {
                    if (nowShowItems[i] != null)
                        PoolMgr.Instance.BackObj(itemResName, nowShowItems[i]);
                    nowShowItems.Remove(i);
                }
            }
            //删除下一节溢出部分
            for (int i = maxIndex + 1; i <= oldMaxIndex; ++i)
            {
                if (nowShowItems.ContainsKey(i))
                {
                    if (nowShowItems[i] != null)
                        PoolMgr.Instance.BackObj(itemResName, nowShowItems[i]);
                    nowShowItems.Remove(i);
                }
            }
        }

        //重置索引
        oldMinIndex = minIndex;
        oldMaxIndex = maxIndex;

        //创建指定索引范围内的格子数
        for (int i = minIndex; i <= maxIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
            {
                continue;
            }
            else
            {
                int temp = i;
                nowShowItems.Add(temp, null);
                PoolMgr.Instance.GetObj(itemResName, (obj) =>
                {
                    //当格子创建出来后
                    //设置它的父对象
                    obj.transform.SetParent(content);
                    //重置相对缩放大小
                    obj.transform.localScale = Vector2.one;
                    //重置位置
                    obj.transform.localPosition = new Vector3((temp % col + 0.5f) * itemW, (-temp / col - 2) * (itemH + itemSpace), 0);
                    //更新格子信息
                    obj.GetComponent<K>().InitInfo(items[temp]);

                    //判断有没有这个坑位
                    if (nowShowItems.ContainsKey(temp))
                        nowShowItems[temp] = obj;
                    else
                        PoolMgr.Instance.BackObj(itemResName, obj);
                });
            }
        }
    }
}
