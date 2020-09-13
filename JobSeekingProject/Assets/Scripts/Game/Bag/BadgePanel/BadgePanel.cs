using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BadgePanel : BasePanel
{
    public GameObject badgeEquiped;
    public GameObject grooves;
    public GameObject badgeGrid;

    private int rowNum = 0;
    private int coluNum = 0;
    private bool hadListener = false;
    private GameObject seleObj;
    private Dictionary<int, List<GameObject>> seleGrid = new Dictionary<int, List<GameObject>>();
    private List<ItemCell> badgeGridList = new List<ItemCell>();
    private List<ItemCell> badgeEquipList = new List<ItemCell>();

    public override void ShowMe()
    {
        base.ShowMe();
        PanelSlideIn();

        //根据已有护符更新护符栏
        CreateBadgeGrid();
        //生成装备的护符,但是这里的参数是没意义的
        CreateEquipedGrid();
        //生成持有的凹槽和已用的凹槽
        CreateGrooves();

        //根据已有格子创造选择框可运动格子
        CreateSeleGrid();

        //选择框
        seleObj = GetControl<Image>("imgSele").gameObject;
        seleObj.transform.SetParent(seleGrid[0][0].gameObject.transform);
        seleObj.transform.position = Vector2.zero;

        //更新InfoPanel的信息
        Invoke("CheckSeleObjPos", 0.1f);

        //增加panelchange监听
        EventCenter.Instance.AddEventListener<int>("PanelChange", PanelSlideOut);
        
    }

    /// <summary>
    /// 更新已解锁护符栏
    /// </summary>
    private void CreateBadgeGrid()
    {
        List<ItemInfo> badgeInfo = new List<ItemInfo>(new ItemInfo[40]);
        foreach(ItemInfo temp in BadgeMgr.Instance.badgeUnlocked.Values)
        {
            badgeInfo[(temp.id - 21)] = temp;
        }

        for(int i = 0; i < badgeGridList.Count; ++i)
        {
            Destroy(badgeGridList[i].gameObject);
            badgeGridList[i].transform.SetParent(null);
        }
        badgeGridList.Clear();
        for(int i = 0; i < badgeInfo.Count; ++i)
        {
            ItemCell cell = ResMgr.Instance.Load<GameObject>("ItemCell").GetComponent<ItemCell>();
            cell.transform.SetParent(badgeGrid.transform);
            cell.InitInfo(badgeInfo[i]);
            if (BadgeMgr.Instance.GetEquipedState(badgeInfo[i]))
                cell.ItemOn();
            else
                cell.ItemOff();
            badgeGridList.Add(cell);
        }
    }

    /// <summary>
    /// 更新已装备护符栏
    /// </summary>
    private void CreateEquipedGrid()
    {
        for (int i = 0; i < badgeEquipList.Count; ++i)
        {
            GameObject.Destroy(badgeEquipList[i].gameObject);
            badgeEquipList[i].gameObject.transform.SetParent(null);
        }
        badgeEquipList.Clear();

        foreach(ItemInfo info in BadgeMgr.Instance.badgeEquiped.Values)
        {
            ItemCell cell = ResMgr.Instance.Load<GameObject>("ItemCell").GetComponent<ItemCell>();
            cell.transform.SetParent(badgeEquiped.transform);
            cell.InitInfo(info);
            badgeEquipList.Add(cell);
        }
    }

    /// <summary>
    /// 更新凹槽栏
    /// </summary>
    private void CreateGrooves()
    {
        int used = GameDataMgr.Instance.playerInfo.GroUsed;
        int unused = GameDataMgr.Instance.playerInfo.GroHeld - used;
        for (int i = 0; i < grooves.transform.childCount; ++i)
        {
            GameObject.Destroy(grooves.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < used; ++i)
        {
            ResMgr.Instance.LoadAsync<GameObject>("groUsed", (o) => {
                o.transform.SetParent(grooves.transform);
            });
        }
        for (int i = 0; i < unused; ++i)
        {
            ResMgr.Instance.LoadAsync<GameObject>("groUnused", (o) => {
                o.transform.SetParent(grooves.transform);
            });
        }
    }

    /// <summary>
    /// 用字典记录选择框行径数据
    /// </summary>
    private void CreateSeleGrid()
    {
        seleGrid.Clear();
        for (int i = 0; i < badgeGrid.transform.childCount; ++i)
        {
            int temp = i / 10;
            if (!seleGrid.ContainsKey(temp))
                seleGrid.Add(temp, new List<GameObject>());

            seleGrid[temp].Add(badgeGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < badgeEquiped.transform.childCount; ++i)
        {
            int temp = i / 10 - 1;
            if (!seleGrid.ContainsKey(temp))
                seleGrid.Add(temp, new List<GameObject>());
            seleGrid[temp].Add(badgeEquiped.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 监听按钮
    /// </summary>
    private void CheckInput(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                //当光标处于左右箭头时，上下无响应
                if (coluNum == -1 || coluNum == 10)
                    return;
                if (rowNum == 0)
                    coluNum = 0;
                rowNum -= 1;
                break;
            case KeyCode.S:
                //当光标处于左右箭头时，上下无响应
                if (coluNum == -1 || coluNum == 10)
                    return;
                if (rowNum == -1)
                    coluNum = 0;
                rowNum += 1;
                break;
            case KeyCode.A:
                //当光标处于左箭头并且向左时，发布切换面板的信息
                if (coluNum == -1)
                    BadgeOnOff();
                coluNum -= 1;
                break;
            case KeyCode.D:
                //当光标处于右箭头并且向右时，发布切换面板的信息
                if (coluNum == 10)
                    BadgeOnOff();
                if (rowNum == -1 && coluNum == seleGrid[rowNum].Count-1)
                    return;
                coluNum += 1;
                break;
            case KeyCode.Space:
                BadgeOnOff();
                break;
        }
        MusicMgr.Instance.PlaySound("UIButton_Selected", false);
        CheckSeleObjPos();
    }

    /// <summary>
    /// 检查自身位置，并触发更新事件
    /// </summary>
    private void CheckSeleObjPos()
    {
        //处理数字
        int rowMin = seleGrid.ContainsKey(-1) ? -1 : 0;
        int rowMax = 3;
        if (rowNum > rowMax)
            rowNum = rowMax;
        if(rowNum < rowMin)
            rowNum = rowMin;

        Transform father;
        if (coluNum < 0)
        {
            father = GetControl<Image>("ArrowLeft").gameObject.transform;
            seleObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 240);
        }
        else if (coluNum > 9)
        {
            father = GetControl<Image>("ArrowRight").gameObject.transform;
            seleObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 240);
        }
        else
        {
            father = seleGrid[rowNum][coluNum].transform;
            seleObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
        }
        seleObj.transform.SetParent(father);
        seleObj.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        ItemCell cell = father.GetComponent<ItemCell>();
        EventCenter.Instance.EventTrigger<ItemInfo>("CurrentPosBadge", cell.GetItemInfo());
    }

    /// <summary>
    /// 发挥功能
    /// </summary>
    private void BadgeOnOff()
    {
        if (seleObj.GetComponentInParent<ItemCell>().GetItemInfo() != null)
        {
            ItemCell father = seleObj.GetComponentInParent<ItemCell>();
            //最后一个是特殊，固有
            if (father.GetItemInfo().id == 60)
                return;

            //交给BadgeMgr判断
            BadgeMgr.Instance.BadgeOnOrOff(father.GetItemInfo());
            //判断现在所选中badge的状态
            if (BadgeMgr.Instance.EquipedJudge)
                father.ItemOn();
            else
                father.ItemOff();
            MusicMgr.Instance.PlaySound("UIBadge_Euqip", false);

            //发生改变，重写选择框路径字典，并且告诉其他三个栏目要更新信息
            if (BadgeMgr.Instance.ListChanged)
            {
                CreateBadgeGrid(); 
                CreateEquipedGrid();
                CreateGrooves();
                CreateSeleGrid();
                seleObj.GetComponent<Image>().enabled = true;
            }
            return;
        }
        if (seleObj.transform.parent.name == "ArrowLeft")
            EventCenter.Instance.EventTrigger<int>("PanelChange", -1);
        if (seleObj.transform.parent.name == "ArrowRight")
            EventCenter.Instance.EventTrigger<int>("PanelChange", 1);
    }

    /// <summary>
    /// 先手面板退出
    /// </summary>
    /// <param name="dir"> 正负分左右</param>
    private void PanelSlideOut(int dir)
    {
        UIMgr.Instance.PopPanel(true);
        seleObj.gameObject.SetActive(false);
        if (hadListener)
        {
            hadListener = false;
            EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
        }
        transform.GetComponent<RectTransform>().DOAnchorPosX(-1920 * dir, 1.1f).onComplete = () =>
        {
            UIMgr.Instance.HidePanel("BadgePanel");
            MainPanel.changinePanel = false;
        };
    }

    /// <summary>
    /// 后手面板进入
    /// </summary>
    private void PanelSlideIn()
    {
        if (transform.GetComponent<RectTransform>().anchoredPosition == Vector2.zero) 
        {
            //添加选择框监听事件
            if (!hadListener)
            {
                hadListener = true;
                EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckInput);
            }
        }
        else
        {
            transform.GetComponent<RectTransform>().DOAnchorPosX(0, 1f).onComplete = () => {
                if (!hadListener)
                {
                    hadListener = true;
                    EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckInput);
                }
            };
        }
    }

    /// <summary>
    /// 隐藏面板时移除监听
    /// </summary>
    public override void HideMe()
    {
        EventCenter.Instance.RemoveEventListener<int>("PanelChange", PanelSlideOut);
        //防止直接按键关闭背包时没有移除按键监听，此处需要再写一次
        if (hadListener)
        {
            hadListener = false;
            EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
        }
    }
}
