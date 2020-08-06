using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventBehaviour : MonoBehaviour
{
    //声明事件
    public event Action attackHandler;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 退出动画
    /// </summary>
    void OnCancelAnim(string animParam)
    {
        anim.SetBool(animParam, false);
    }

    /// <summary>
    /// 事件委托
    /// </summary>
    void OnAttack()
    {
        attackHandler?.Invoke();
    }
}
