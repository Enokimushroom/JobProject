using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

/// <summary>
/// 可以提供给外部添加帧更新事件的方法
/// 可以提供给外部添加协程的方法
/// 让没有继承Mono的类可以开启协程，可以Update更新，统一管理update
/// </summary>
public class MonoMgr : BaseManager<MonoMgr>
{
    private MonoController controller;

    public MonoMgr()
    {
        //保证了MonoController对象的唯一性
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }
    public void AddUpdateListener(UnityAction func)
    {
        controller.updateEvent += func;
    }

    /// <summary>
    /// 给外部提供的 用于移除帧更新事件函数
    /// </summary>
    /// <param name="func"></param>
    public void RemoveUpdateListener(UnityAction func)
    {
        controller.updateEvent -= func;
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }


    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }

    [System.Obsolete]
    public Coroutine StartCoroutine_Auto(IEnumerator routine)
    {
        return controller.StartCoroutine_Auto(routine);
    }

    public void StopAllCoroutines()
    {
        controller.StopAllCoroutines();
    }

    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(string methodName)
    {
        controller.StopCoroutine(methodName);
    }

    public void CallIR(UnityAction func)
    {
        controller.CallIR(func);
    }

    public void ClearUpdateEvent()
    {

    }
}
