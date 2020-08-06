using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 抽屉数据 池子中的一列容器
/// </summary>
public class PoolData
{
    //抽屉中 对象挂在的父节点
    public GameObject fatherObj;

    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        //给抽屉创造一个父对象并且把他作为Pool（衣柜）对象的子物体
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() {  };
        PushObj(obj);
    }

    /// <summary>
    /// 往抽屉里面放东西
    /// </summary>
    public void PushObj(GameObject obj)
    {
        //失活，让其隐藏
        obj.SetActive(false);
        //存起来
        poolList.Add(obj);
        //设置父对象
        obj.transform.SetParent(fatherObj.transform);
    }

    /// <summary>
    /// 从抽屉里面取东西
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        //取出第一个
        obj = poolList[0];
        poolList.RemoveAt(0);
        //激活 让其显示
        obj.SetActive(true);
        //断开了父子关系
        obj.transform.SetParent(null);
        return obj;
    }
}

/// <summary>
/// 缓存池
/// </summary>
public class PoolMgr:BaseManager<PoolMgr>
{
    //缓存池容器
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// 往外拿东西
    /// </summary>
    public GameObject GetObj(string name, UnityAction<GameObject> callback)
    {
        GameObject obj = null;
        //有抽屉并且抽屉里面有东西
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            callback(poolDic[name].GetObj());
        }
        else
        {
            //通过异步加载资源，创建对象给外部用
            ResMgr.Instance.LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name;
                callback(o);
            });
        }

        return obj;
    }

    /// <summary>
    /// 往里存东西
    /// </summary>
    public void BackObj(string name , GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");

        //里面有抽屉
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //里面没有抽屉
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }

    /// <summary>
    /// 清空缓存池
    /// 主要用在场景切换时
    /// </summary>
    public void PoolClear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
