using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{
    //空接口
}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventCenter:BaseManager<EventCenter>
{
    //key-事件的名字
    //value-监听这个事件对应的委托函数们
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听（必须带参数）
    /// </summary>
    /// <param name="name"> 事件的名字 </param>
    /// <param name="action"> 准备用来处理事件的委托函数 </param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //事件中心中有这个对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 添加事件监听（不用带参数）
    /// </summary>
    public void AddEventListener(string name, UnityAction action)
    {
        //事件中心中有这个对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }


    /// <summary>
    /// 移除对应的事件监听（必须要有参数）
    /// 主要在对象的OnDestory中调用，不然对象销毁监听依然在进行会造成内存泄漏
    /// </summary>
    public void RemoveEventListener<T>(string name,UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
        if (eventDic[name] == null)
            eventDic.Remove(name);
    }

    /// <summary>
    /// 移除对应的事件监听（不需要参数）
    /// </summary>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
        if (eventDic[name] == null)
            eventDic.Remove(name);
    }


    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">哪一个名字的事件触发了</param>
    public void EventTrigger<T>(string name,T info)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name].Invoke(Info);
            (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
        }
    }

    /// <summary>
    /// 事件触发（不需要参数）
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name].Invoke(Info);
            (eventDic[name] as EventInfo).actions?.Invoke();
        }
    }

    /// <summary>
    /// 清空事件中心
    /// 主要用于场景切换
    /// </summary>
    public void EventTriggerClear()
    {
        eventDic.Clear();
    }
}
