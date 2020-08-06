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
    public AsyncOperation ao;

    /// <summary>
    /// 切换场景（同步）
    /// </summary>
    public void LoadScene(string name,UnityAction func)
    {
        //场景同步加载
        SceneManager.LoadScene(name);
        //加载完成过后，才会执行func
        func();
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
    /// <param name="name"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction func)
    {
        ao = SceneManager.LoadSceneAsync(name);

        ao.allowSceneActivation = false;
        //可以得到场景加载的一个进度
        while (!ao.isDone)
        {
            //事件中心 向外分发 进度情况 外面想用就用
            EventCenter.Instance.EventTrigger("Loading", ao.progress);
            if (ao.progress >= 0.9f)
            {
                //执行完func后再正式显示新场景
                func();
            }
            //希望在这里面更新进度条
            yield return ao.progress;
        }
    }
}
