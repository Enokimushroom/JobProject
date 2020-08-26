using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LevelManager : BaseManager<LevelManager>
{
    /// <summary>
    /// 当前关卡ID
    /// </summary>
    public string currentLvID ;

    /// <summary>
    /// 此地图是否为关卡副本（以后可以对接地图管理器）
    /// true代表是杀完怪就不自动跳转下一关的普通地图，false代表杀完怪会自动跳转下一波直到完成关卡获得最后奖励的副本地图
    /// </summary>
    private bool dungeon;

    /// <summary>
    /// 当前关卡信息
    /// </summary>
    private LevelInfo.LevelBase currentInfo;
    /// <summary>
    /// 关卡基本信息队列
    /// </summary>
    private Queue<LevelInfo.LevelBase> levelInfos = new Queue<LevelInfo.LevelBase>();

    /// <summary>
    /// 场景中怪物剩余数量
    /// </summary>
    private int enermyLeft;
    /// <summary>
    /// 记录敌人位置的list
    /// </summary>
    private List<EnermyPos> enermyList = new List<EnermyPos>();
    /// <summary>
    /// 当前关卡所有敌人
    /// </summary>
    private Dictionary<string, List<EnermyStatus>> allEnermyInCurrentScene = new Dictionary<string, List<EnermyStatus>>();

    /// <summary>
    /// 怪物事件委托
    /// </summary>
    /// <param name="enermyID"></param>
    public delegate void CheckEnermyStatus(string enermyID);
    public event CheckEnermyStatus OnDeathEvent;

    private Transform enermyGroup;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        dungeon = true;
        currentLvID = GameDataMgr.Instance.playerInfo.MapType.ToString() + GameDataMgr.Instance.playerInfo.MapID.ToString();
    }
    
    /// <summary>
    /// 更新关卡ID
    /// </summary>
    public void SetCurrentLevelID(string level)
    {
        currentLvID = level;
    }

    /// <summary>
    /// 读取关卡信息（交给场景管理器调用）
    /// </summary>
    /// <param name="lvID"></param>
    public void EnqueueLevel()
    {
        if (currentLvID == string.Empty) return;
        //读取相对应的scriptableObject
        LevelInfo temp = ResMgr.Instance.Load<LevelInfo>(currentLvID);
        if (temp != null)
        {
            dungeon = temp.isDungeon;
            //将相应的波数依次存入队列中
            foreach (LevelInfo.LevelBase info in temp.levelInfo)
            {
                levelInfos.Enqueue(info);
            }
        }
        DequeueLevel();
    }

    /// <summary>
    /// 输出关卡信息
    /// </summary>
    public void DequeueLevel()
    {
        if (levelInfos.Count == 0)
        {
            //表示已经打通了当前副本
            //结束逻辑
            if (currentInfo != null)
                currentInfo.onFinish?.Invoke();
            //奖励
            return;
        }

        //获取当前关卡信息
        currentInfo = levelInfos.Dequeue();
        //清空列表并依次读取怪物信息
        enermyList.Clear();
        foreach (EnermyPos emInfo in currentInfo.enermyArray)
        {
            enermyList.Add(emInfo);
        }
        enermyLeft = enermyList.Count;

        //检查开始事件
        if (currentInfo.onStart.GetPersistentEventCount() != 0 && !dungeon)
            currentInfo.onStart?.Invoke();
        else
        {
            CreateGenerator();
        }
    }

    /// <summary>
    /// 根据GenerateType生成不同的生成器
    /// </summary>
    public void CreateGenerator()
    {
        for (int i = 0; i < enermyList.Count; ++i)
        {
            int temp = i;
            switch (enermyList[i].enermyType)
            {
                case GenerateType.SmallBox:
                    //小生成器
                    CreateEnermy("SmallGenerator", temp);
                    break;
                case GenerateType.BigBox:
                    //大生成器
                    CreateEnermy("BigGenerator", temp);
                    break;
                case GenerateType.None:
                    //无生成器
                    ResMgr.Instance.LoadAsync<GameObject>(enermyList[temp].enermyName, (o) =>
                    {
                        if (enermyGroup == null)
                            enermyGroup = GameObject.Find("Enermy").transform;
                        o.transform.SetParent(enermyGroup);
                        o.transform.position = enermyList[temp].enermyPos;
                        EnermyStatus es = o.GetComponentInChildren<EnermyStatus>();
                        AddEnermyDic(es);
                        es.OnDeathEvent += EnermyDied;
                    });
                    break;
            }
        }
    }

    /// <summary>
    /// 生成敌人
    /// </summary>
    private void CreateEnermy(string generatorName, int temp)
    {
        //生成不同生成器
        ResMgr.Instance.LoadAsync<GameObject>(generatorName, (obj) =>
        {
            if (enermyGroup == null)
                enermyGroup = GameObject.Find("Enermy").transform;
            obj.transform.SetParent(enermyGroup);
            //设置生成器初始位置并记录下来
            obj.transform.position = new Vector2(enermyList[temp].enermyPos.x, -10);
            Vector2 tempPos = obj.transform.position;
            //生成器移动动画
            obj.transform.DOMoveY(enermyList[temp].enermyPos.y, 1.0f).onComplete = () =>
            {
                //动画结束回调函数
                EnermyGenerator tempObj = obj.GetComponent<EnermyGenerator>();
                tempObj.generatePos = tempPos;
                //添加到生成动画中的事件
                tempObj.generatorHandler += () => {
                    //根据名字生成不同的怪物预制体
                    ResMgr.Instance.LoadAsync<GameObject>(enermyList[temp].enermyName, (o) =>
                    {
                        //初始化正确的生成位置
                        o.transform.position = enermyList[temp].enermyPos;
                        EnermyStatus es = o.GetComponent<EnermyStatus>();
                        //添加生成的怪物到记录当前场景怪物的字典中并添加订阅事件
                        AddEnermyDic(es);
                        es.OnDeathEvent += EnermyDied;
                    });
                };
                //播放生成动画
                tempObj.BoolAnim("Generate");
            };
        });
    }

    /// <summary>
    /// 添加怪物到当前场景怪物的记录字典中
    /// </summary>
    private void AddEnermyDic(EnermyStatus es)
    {
        if (es == null) return;
        if (!allEnermyInCurrentScene.ContainsKey(es.EnermyID))
        {
            allEnermyInCurrentScene.Add(es.EnermyID, new List<EnermyStatus>());
        }
        allEnermyInCurrentScene[es.EnermyID].Add(es);
    }

    /// <summary>
    /// 怪物死亡触发事件
    /// </summary>
    private void EnermyDied(EnermyStatus es)
    {
        //当前怪物数目减一
        enermyLeft--;
        //任务系统转接过来的事件触发（判断死亡的怪物是不是任务需要的那个）
        OnDeathEvent?.Invoke(es.EnermyID);
        //从当前场景怪物记录字典中移除这个怪物
        allEnermyInCurrentScene[es.EnermyID].Remove(es);
        //如果当前场景怪物为0并且为副本关卡，读取下一波
        if (enermyLeft == 0 && !dungeon)
        {
            DequeueLevel();
        }
    }

    public void Reset()
    {
        enermyGroup = null;
    }
}
