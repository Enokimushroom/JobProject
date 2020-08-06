using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoController : MonoBehaviour
{
    public event UnityAction updateEvent;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    /// <param name="func"></param>
    public void AddUpdateListener(UnityAction func)
    {
        updateEvent += func;
    }

    /// <summary>
    /// 给外部提供的 用于移除帧更新事件函数
    /// </summary>
    /// <param name="func"></param>
    public void RemoveUpdateListener(UnityAction func)
    {
        updateEvent -= func;
    }

    public void CallIR(UnityAction func)
    {
        irFunc = func;
        InvokeRepeating("IRFunc", 0, 0.5f);
    }

    UnityAction irFunc;
    public void IRFunc()
    {
        irFunc();
    }
}
