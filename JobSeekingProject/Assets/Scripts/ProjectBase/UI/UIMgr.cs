using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Rendering;

/// <summary>
/// UI层级
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    top,
    system,
}

public enum ConfirmType
{
    OneBtn,
    TwoBtn,
}

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等等接口
/// </summary>
public class UIMgr : BaseManager<UIMgr>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Stack<BasePanel> panelStack;

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    private GameObject crossFade;

    public UIMgr()
    {
        //找到Canva，让其过场景不被移除
        GameObject obj = ResMgr.Instance.Load<GameObject>("Canvas");
        Transform canvas = obj.transform;
        GameObject.DontDestroyOnLoad(obj);

        //找到各层
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //创建EventSystem让其过场景的时候不被移除
        obj = ResMgr.Instance.Load<GameObject>("EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"> 面板脚本类型 </typeparam>
    /// <param name="panelName"> 面板名 </param>
    /// <param name="layer"> 显示在哪一层 </param>
    /// <param name="callBack"> 当面板预设体创建之后你想做的事情</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            if (callBack != null)
            {
                callBack(panelDic[panelName] as T);
            }
            else
                return;
        }

        //放入栈管理
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();
        //判断一下栈里面是否有界面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        ResMgr.Instance.LoadAsync<GameObject>(panelName, (obj) =>
        {
            //把他作为canvas的子对象
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.top:
                    father = top;
                    break;
                case E_UI_Layer.system:
                    father = system;
                    break;
            }
            //设置父对象
            obj.transform.SetParent(father);
            //并且要设置它的相对位置
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            //处理面板创建完成后的逻辑
            callBack?.Invoke(panel);
            panel.ShowMe();
            panelDic.Add(panelName, panel);
            //入栈
            panelStack.Push(panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
        else
        {
            Debug.Log(panelName + "不存在");
        }
    }

    /// <summary>
    /// 出栈
    /// </summary>
    public void PopPanel(bool delay=false)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel = panelStack.Pop();
        if (!delay)
        {
            string panelName = topPanel.name.Replace("(Clone)", string.Empty);
            HidePanel(panelName);
        }

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
    }

    /// <summary>
    /// 清空栈内容
    /// </summary>
    public void ClearPanelStack()
    {
        int times = panelStack.Count;
        for(int i = 0; i < times; ++i)
        {
            BasePanel bp = panelStack.Pop();
            string name = bp.name.Replace("(Clone)", string.Empty);
            HidePanel(name);
        }
        
    }

    /// <summary>
    /// 创建子物体（一般用于父物体有Grid，不用调整位置）
    /// </summary>
    public void CreatChildren(string childName, GameObject father,float index)
    {
        Transform transform;
        for (int i = 0; i < father.transform.childCount; ++i)
        {
            transform = father.transform.GetChild(i);
            GameObject.Destroy(transform.gameObject);
        }

        for(int i = 0; i < index; ++i)
        {
            ResMgr.Instance.LoadAsync<GameObject>(childName, (obj) => 
            {
                obj.transform.SetParent(father.transform);
            });
        }
    }

    /// <summary>
    /// 获取物品时显示拾取提示UI
    /// </summary>
    /// <param name="info"></param>
    /// <param name="layer"></param>
    public void CommonHint(ItemInfo info)
    {
        Item item = GameDataMgr.Instance.GetItemInfo(info.id);
        ResMgr.Instance.LoadAsync<GameObject>("CommonItemHint",(o)=>
        {
            o.transform.SetParent(top);
            o.GetComponent<RectTransform>().anchoredPosition = new Vector3(-150, 70, 0);
            o.transform.GetChild(0).GetComponent<Image>().sprite = ResMgr.Instance.Load<Sprite>(item.icon);
            o.transform.GetChild(1).GetComponent<Text>().text = item.name;
            GameObject.Destroy(o, 4.0f);
        });
    }

    /// <summary>
    /// 进入相关地图时的地图提示UI
    /// </summary>
    public void MapHint(string mapDes,string mapName)
    {
        ResMgr.Instance.LoadAsync<GameObject>("MapHint", (o) =>
        {
            o.transform.SetParent(top);
            o.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 253, 0);
            o.transform.GetChild(0).GetComponent<Text>().text = mapDes;
            o.transform.GetChild(1).GetComponent<Text>().text = mapName;
            GameObject.Destroy(o, 4.0f);
        });
    }

    public void MapHintTxt(string des)
    {
        ResMgr.Instance.LoadAsync<GameObject>("MapHintTxt", (o) => 
        {
            o.transform.SetParent(top);
            o.GetComponent<RectTransform>().anchoredPosition = new Vector3(350, 175, 0);
            o.GetComponent<Text>().text = des;
            GameObject.Destroy(o, 4.0f);
        });
    }

    /// <summary>
    /// 提供两种样式确认面板的方法
    /// </summary>
    public void ShowConfirmPanel(string confirmQuestion, ConfirmType ct, UnityAction yes, UnityAction no = null)
    {
        string panelName;
        switch (ct)
        {
            case ConfirmType.OneBtn:
                panelName = "oneBtnTipPanel";
                break;
            case ConfirmType.TwoBtn:
                panelName = "twoBtnTipPanel";
                break;
            default:
                panelName = null;
                break;
        }
        PlayerStatus.Instance.InputEnable = false;
        ShowPanel<BasePanel>(panelName, E_UI_Layer.system,(p)=> 
        {
            switch (ct)
            {
                case ConfirmType.OneBtn:
                    p.GetComponent<OneBtnTipPanel>().InitInfo(confirmQuestion, no);
                    break;
                case ConfirmType.TwoBtn:
                    p.GetComponent<TwoBtnTipPanel>().InitInfo(confirmQuestion, yes, no);
                    break;
            }
        });
        

    }

}
