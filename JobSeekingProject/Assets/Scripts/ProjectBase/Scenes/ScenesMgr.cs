using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// </summary>
public class ScenesMgr : UnityBaseManager<ScenesMgr>
{
    //public AsyncOperation ao;
    private int displayProcess;
    private int currentProcess;
    public bool goingScene { get; set; }

    /// <summary>
    /// 切换场景（同步）
    /// </summary>
    public void LoadScene(string name,UnityAction func)
    {
        ResetMgr();
        //场景同步加载
        SceneManager.LoadScene(name);
        //加载完成过后，才会执行func
        func?.Invoke();
    }

    /// <summary>
    /// 切换场景（异步）
    /// </summary>
    public void LoadSceneAsyn(string name, UnityAction func)
    {
        StartCoroutine(ReallyLoadSceneAsyn(name, func));
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction func)
    {
        //重置进度数值
        displayProcess = 0;
        //为了稍微减点gc
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        ResetMgr();
        //loading页面进栈
        UIMgr.Instance.ShowPanel<BasePanel>("LoadingPanel", E_UI_Layer.system);
        yield return delay;
        //开始异步加载
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        ao.allowSceneActivation = false;

        while (ao.progress < 0.9f)
        {
            currentProcess = (int)(ao.progress * 100);
            while (displayProcess < currentProcess)
            {
                displayProcess++;
                //更新Loading页面的进度条
                EventCenter.Instance.EventTrigger<int>("SceneLoadingProcess", displayProcess);
                yield return delay;
            }
            yield return delay;
        }

        currentProcess = 100;
        while (displayProcess < currentProcess)
        {
            displayProcess++;
            //更新Loading页面的进度条
            EventCenter.Instance.EventTrigger<int>("SceneLoadingProcess", displayProcess);
            yield return delay;
        }
        UIMgr.Instance.PopPanel();
        ao.allowSceneActivation = true;
        yield return delay;

        //执行回调
        func?.Invoke();
    }

    /// <summary>
    /// 切换场景时重置管理器相关信息
    /// </summary>
    private void ResetMgr()
    {
        MusicMgr.Instance.MusicClear();
        PEManager.Instance.Clear();
        MonoMgr.Instance.StopAllCoroutines();
        EventCenter.Instance.EventTriggerClear();
        PoolMgr.Instance.PoolClear();
        LevelManager.Instance.Reset();
    }
}
