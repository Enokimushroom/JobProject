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
    /// 攻击事件委托
    /// </summary>
    void OnAttack()
    {
        attackHandler?.Invoke();
    }

    /// <summary>
    /// 暂停输入
    /// </summary>
    public void PauseInput()
    {
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.IsForzen = true;
        PlayerStatus.Instance.EnableGravity = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    /// <summary>
    /// 继续输入
    /// </summary>
    public void ContinueInput()
    {
        PlayerStatus.Instance.InputEnable = true;
        PlayerStatus.Instance.IsForzen = false;
        PlayerStatus.Instance.EnableGravity = true;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void Reborn()
    {
        GameManager.Instance.RebornPlayer();
    }
}
