using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommonPanel : BasePanel
{
    public GameObject fixGrid;
    public GameObject skillGrid;
    public GameObject itemGrid;

    private int row = 0;
    private int col = 0;
    private GameObject seleObj;
    private Dictionary<int, List<GameObject>> seleGrid = new Dictionary<int, List<GameObject>>();

    public override void ShowMe()
    {
        base.ShowMe();
        PanelSlideIn();

        //触发初始化事件
        //更新fixItem
        //更新skillItem
        //更新numItem
        EventCenter.Instance.EventTrigger("InitInfo");
        CreateFixItem();

        //根据已有格子创建选择框可运动格子
        CreateSeleGrid();

        //选择框
        seleObj = GetControl<Image>("imgSele").gameObject;
        seleObj.transform.SetParent(seleGrid[0][0].gameObject.transform);
        seleObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //更新InfoPanel的信息
        Invoke("CheckSeleObjPos", 0.1f);

        //增加panelChange的监听
        EventCenter.Instance.AddEventListener<int>("PanelChange", PanelSlideOut);
    }

    private void CreateFixItem()
    {
        for(int i = 0; i < fixGrid.transform.childCount; ++i)
        {
            fixGrid.transform.GetChild(i).GetComponent<FixItemCell>().CheckFixItem();
        }
    }

    /// <summary>
    /// 用字典记录选择框行径数据
    /// </summary>
    private void CreateSeleGrid()
    {
        seleGrid.Clear();
        for(int i = 0; i < 6; ++i)
        {
            int temp = i / 3;
            if (!seleGrid.ContainsKey(temp))
                seleGrid.Add(temp, new List<GameObject>());
            seleGrid[temp].Add(fixGrid.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < skillGrid.transform.childCount; ++i)
        {
            int temp = i / 4;
            seleGrid[temp].Add(skillGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < itemGrid.transform.childCount; ++i)
        {
            //int temp = Mathf.CeilToInt((skillGrid.transform.childCount / 4));
            int temp = (int)Math.Ceiling((double)skillGrid.transform.childCount / 4);
            if (!seleGrid.ContainsKey(temp))
                seleGrid.Add(temp, new List<GameObject>());
            seleGrid[temp].Add(itemGrid.transform.GetChild(i).gameObject);
        }

        seleGrid.Add(3, new List<GameObject>() { fixGrid.transform.GetChild(6).gameObject });
        seleGrid.Add(4, new List<GameObject>() { fixGrid.transform.GetChild(7).gameObject });
        seleGrid.Add(5, new List<GameObject>() { fixGrid.transform.GetChild(8).gameObject });
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
                if (row == -1 || col == seleGrid[row].Count)
                    return;
                if (row == 3)
                {
                    row = 2;
                    col = 1;
                }

                row -= 1;
                break;
            case KeyCode.S:
                //当光标处于左右箭头时，上下无响应
                if (col == -1 || col == seleGrid[row].Count)
                    return;
                if (row == 1)
                {
                    switch (col)
                    {
                        case 0:
                            row = 4;
                            col = 0;
                            break;
                        case 1:
                            row = 2;
                            col = 0;
                            break;
                        case 2:
                            row = 2;
                            col = 0;
                            break;
                    }
                }


                if (row == (int)Math.Ceiling((double)skillGrid.transform.childCount / 4))
                    return;

                row += 1;
                break;
            case KeyCode.A:
                //当光标处于左箭头并且向左时，发布切换面板的信息
                if (col == -1)
                    PanelChange();
                if (col == 3)
                    row = 0;
                if (row >= 3)
                {
                    row = 1;
                    col = 1;
                }


                col -= 1;
                break;
            case KeyCode.D:
                //当光标处于右箭头并且向右时，发布切换面板的信息
                if (col == seleGrid[row].Count)
                    PanelChange();
                if (col == 2)
                    row = 0;
                if (row >= 3)
                {
                    row = 0;
                    col = 2;
                }

                col += 1;
                break;
            case KeyCode.Space:
                PanelChange();
                break;
        }
        CheckSeleObjPos();
    }

    /// <summary>
    /// 检查自身位置，并触发更新事件
    /// </summary>
    private void CheckSeleObjPos()
    {
        int rowMin = 0;
        int rowMax = 5;
        if (row > rowMax)
            row = rowMax;
        if (row < rowMin)
            row = rowMin;



        Transform father;
        if (col < 0)
        {
            father = GetControl<Image>("ArrowLeft").gameObject.transform;
            seleObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 240);
        }
        else if (col > seleGrid[row].Count - 1)
        {
            father = GetControl<Image>("ArrowRight").gameObject.transform;
            seleObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 240);
        }
        else
        {
            father = seleGrid[row][col].transform;
            seleObj.transform.GetComponent<RectTransform>().sizeDelta = father.GetComponent<RectTransform>().sizeDelta + new Vector2(60, 60);
        }
        seleObj.transform.SetParent(father);
        seleObj.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        FixItemCell cell = father.GetComponent<FixItemCell>();
        EventCenter.Instance.EventTrigger<ItemInfo>("CurrentPosCommon", cell.GetItemInfo());
    }

    public override void HideMe()
    {
        EventCenter.Instance.RemoveEventListener<int>("PanelChange", PanelSlideOut);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
    }

    /// <summary>
    /// 先手面板退出
    /// </summary>
    /// <param name="dir"> 正负分左右</param>
    void PanelSlideOut(int dir)
    {
        UIMgr.Instance.PopPanel(true);
        seleObj.gameObject.SetActive(false);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
        transform.GetComponent<RectTransform>().DOAnchorPosX(-1920 * dir, 1.1f).onComplete = () => {
            UIMgr.Instance.HidePanel("CommonPanel");
        };
    }


    /// <summary>
    /// 后手面板进入
    /// </summary>
    void PanelSlideIn()
    {
        this.GetComponent<RectTransform>().DOAnchorPosX(0, 1f).onComplete = () => {
            //添加选择框监听事件
            EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckInput);
        };

    }

    void PanelChange()
    {
        if (seleObj.transform.parent.name == "ArrowLeft")
            EventCenter.Instance.EventTrigger<int>("PanelChange", -1);
        if (seleObj.transform.parent.name == "ArrowRight")
            EventCenter.Instance.EventTrigger<int>("PanelChange", 1);
    }
}
