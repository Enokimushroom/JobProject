using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// 资源加载模块
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    private Dictionary<string, string> configMap;
    private string fileContent;

    //public ResMgr()
    //{
    //    //加载映射表
    //    fileContent = GetConfigFile("ConfigMap.txt");
    //    //解析文件（string  -->  Dictionary<string,string>)
    //    if (fileContent != null)
    //        BuildMap(fileContent);
    //    else
    //        UnityEngine.Debug.Log("读取失败，请重试。");
    //}

    public void Init()
    {
        //加载映射表
        fileContent = GetConfigFile("ConfigMap.txt");
        //解析文件（string  -->  Dictionary<string,string>)
        if (fileContent != null)
            BuildMap(fileContent);
        else
            UnityEngine.Debug.Log("读取失败，请重试。");
    }

    private string GetConfigFile(string fileName)
    {
        //string url = "file://" + Application.streamingAssetsPath + "/ConfigMap.txt";
        UnityWebRequest request = UnityWebRequest.Get("file://" + Application.streamingAssetsPath + "/" + fileName);
        request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            UnityEngine.Debug.Log(request.error);
            return null;
        }
        else
        {
            UnityEngine.Debug.Log("映射表读取成功");
            return request.downloadHandler.text;
        }
    }

    private void BuildMap(string fileContent)
    {
        configMap = new Dictionary<string, string>();
        //文件名=路径/r/n
        //StringReader字符串读取器，提供逐行读取字符串的功能
        using (StringReader reader = new StringReader(fileContent))
        {
            string line = reader.ReadLine();
            while (line!=null)
            {
                string[] keyValue = line.Split('=');
                if (configMap.ContainsKey(keyValue[0]))
                    UnityEngine.Debug.Log(keyValue[0] + "已经存在，请及时修正。地址：" + keyValue[1]);
                configMap.Add(keyValue[0], keyValue[1]);
                line = reader.ReadLine();
            }
        }
        UnityEngine.Debug.Log("映射表建造成功");
        //退出using代码块，会自动reader.Dispose()
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    public T Load<T>(string name) where T:Object 
    {
        if (!configMap.ContainsKey(name))
        {
            UnityEngine.Debug.Log("资源加载字典查无此物:" + name);
            return null;
        }
        string path = configMap[name];
        T res = Resources.Load<T>(path);
        //如果对象是一个GameObject类型的，要实例化后再返回出去 外部直接使用即可
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    public void LoadAsync<T>(string name,UnityAction<T> callback) where T: Object
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadAsync(name, callback));
    }

    /// <summary>
    /// 真正的协同程序函数，用于开启异步加载资源
    /// </summary>
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        string path = configMap[name];
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;

        if (r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }

    //lambda表达式用于异步后的协程函数，可以用lambda避免多次写方法函数
}
