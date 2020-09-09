using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HunterPanel : BasePanel
{
    //履带对象  通过他得到可视范围的位置  还要动态创建格子设置为它的子对象
    public RectTransform content;

    Mugen<ItemInfo, HunterCell> sv;

    //选择框物体
    private GameObject seleObj;

    private int seleIndex = -1;
    private int oldIndex = 0;

    public override void ShowMe()
    {
        base.ShowMe();
        PanelSlideIn();

        sv = new Mugen<ItemInfo, HunterCell>();
        sv.InitCotentAndSVH(content, 700);
        sv.InitItemSizeAndCol(450, 120, 20, 1);
        sv.InitItemResName("HunterCell");
        sv.InitInfos(GameDataMgr.Instance.playerInfo.hunterList);

        //显示面板时，更新格子信息
        sv.CheckShowOrHide();

        //选择框
        seleObj = GetControl<Image>("imgSele").gameObject;
        seleIndex = sv.seleIndex;

        //更新TipsPanel的信息
        Invoke("CheckSeleObjPos", 0.5f);

        //增加PanelChange的监听
        EventCenter.Instance.AddEventListener<int>("PanelChange", PanelSlideOut);
    }

    private void CheckInput(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                if (seleIndex == 0) return;
                content.anchoredPosition += new Vector2(0, -140);
                sv.CheckShowOrHide();
                seleIndex = sv.seleIndex;
                break;
            case KeyCode.S:
                if (seleIndex == GameDataMgr.Instance.playerInfo.hunterList.Count - 1) return;
                content.anchoredPosition += new Vector2(0, 140);
                sv.CheckShowOrHide();
                seleIndex = sv.seleIndex;
                break;
            case KeyCode.A:
                if (seleIndex >= 0)
                {
                    oldIndex = seleIndex;
                    seleIndex = -1;
                }
                else if (seleIndex == -1)
                {
                    PanelChange();
                }
                else
                {
                    seleIndex = oldIndex;
                }
                break;

            case KeyCode.D:
                if (seleIndex >= 0)
                {
                    oldIndex = seleIndex;
                    seleIndex = -2;
                }
                else if (seleIndex == -1)
                {
                    seleIndex = oldIndex;
                }
                else
                {
                    PanelChange();
                }
                break;
            case KeyCode.Space:
                PanelChange();
                break;
        }
        MusicMgr.Instance.PlaySound("UIButton_Selected", false);
        CheckSeleObjPos();
    }

    private void CheckSeleObjPos()
    {
        Transform father;
        if (seleIndex == -1)
        {
            father = GetControl<Image>("ArrowLeft").gameObject.transform;
            GetControl<Image>("imgSele").sprite = ResMgr.Instance.Load<Sprite>("Selection");
            seleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 240);
        }
        else if (seleIndex == -2)
        {
            father = GetControl<Image>("ArrowRight").gameObject.transform;
            GetControl<Image>("imgSele").sprite = ResMgr.Instance.Load<Sprite>("Selection");
            seleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 240);
        }
        else
        {
            father = sv.nowShowItems[seleIndex].transform;
            GetControl<Image>("imgSele").sprite = ResMgr.Instance.Load<Sprite>("SeleObj");
            seleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
        }
        seleObj.transform.SetParent(father);
        seleObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        HunterCell cell = father.GetComponent<HunterCell>();

        if (cell != null)
            EventCenter.Instance.EventTrigger<ItemInfo>("CurrentPosHunterTip", cell.GetItemInfo());
        else
            EventCenter.Instance.EventTrigger<ItemInfo>("CurrentPosHunterTip", null);
    }

    public override void HideMe()
    {
        EventCenter.Instance.RemoveEventListener<int>("PanelChange", PanelSlideOut);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
    }

    void PanelSlideIn()
    {
        this.GetComponent<RectTransform>().DOAnchorPosX(0, 1f).onComplete = () => {
            //添加选择框监听事件
            EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckInput);
        };
    }

    void PanelSlideOut(int dir)
    {
        UIMgr.Instance.PopPanel(true);
        seleObj.gameObject.SetActive(false);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckInput);
        transform.GetComponent<RectTransform>().DOAnchorPosX(-1920 * dir, 1.1f).onComplete = () => {
            UIMgr.Instance.HidePanel("HunterPanel");
            MainPanel.changinePanel = false;
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
