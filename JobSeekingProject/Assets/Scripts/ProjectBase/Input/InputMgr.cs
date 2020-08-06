using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart = false;
    
    /// <summary>
    /// 构造函数中添加Update监听
    /// </summary>
    public InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// 是否开启或者关闭我的输入检测
    /// </summary>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    /// <summary>
    /// 用来检测按键抬起按下，分发事件
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {

        //事件中心模块 分发按下事件
        if (Input.GetKeyDown(key))
        {
            EventCenter.Instance.EventTrigger("xPress", key);
        }
        //事件中心模块 分发抬起事件
        if (Input.GetKeyUp(key))
        {
            EventCenter.Instance.EventTrigger("xUp", key);
        }

        if (Input.GetKey(key))
        {
            EventCenter.Instance.EventTrigger("xPressing", key);
        }
    }

    private void MyUpdate()
    {
        if (!isStart)
            return;

        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
        CheckKeyCode(KeyCode.J);
        CheckKeyCode(KeyCode.K);
        CheckKeyCode(KeyCode.L);
        CheckKeyCode(KeyCode.Space);
        CheckKeyCode(KeyCode.B);
        CheckKeyCode(KeyCode.Escape);
        CheckKeyCode(KeyCode.O);
        CheckKeyCode(KeyCode.Q);
        CheckKeyCode(KeyCode.E);
        CheckKeyCode(KeyCode.R);
        CheckKeyCode(KeyCode.Z);
        CheckKeyCode(KeyCode.C);
        CheckKeyCode(KeyCode.X);
        CheckKeyCode(KeyCode.V);
        CheckKeyCode(KeyCode.N);
        CheckKeyCode(KeyCode.M);
        CheckKeyCode(KeyCode.F);
        CheckKeyCode(KeyCode.G);
        CheckKeyCode(KeyCode.H);
        CheckKeyCode(KeyCode.T);
        CheckKeyCode(KeyCode.Y);
        CheckKeyCode(KeyCode.U);
        CheckKeyCode(KeyCode.I);
        CheckKeyCode(KeyCode.P);
    }
}
