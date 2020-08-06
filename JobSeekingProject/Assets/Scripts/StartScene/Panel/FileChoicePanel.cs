using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

public class FileChoicePanel : BasePanelInStartScene
{
    public enum FileChoiceMenuButton
    {
        FileOne,
        FileTwo,
        FileThree,
        FileFour,
        Back,
        ClearOne,
        ClearTwo,
        ClearThree,
        ClearFour,
    }

    public Transform[] lifeGridFile;
    private string fileOnePath;
    private string fileTwoPath;
    private string fileThreePath;
    private string fileFourPath;
    private Animator anim;

    public override void ShowMe()
    {
        base.ShowMe();
        anim = GetComponent<Animator>();

        fileOnePath = CheckSaveFiles(1);
        fileTwoPath = CheckSaveFiles(2);
        fileThreePath = CheckSaveFiles(3);
        fileFourPath = CheckSaveFiles(4);
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    private string CheckSaveFiles(int num)
    {
        string saveDir = GameDataMgr.Instance.PlayerInfo_Url;
        string saveNumDir = saveDir + "/save" + num.ToString();
        string playerSave = saveNumDir + "/playerInfo.txt";
        if (!Directory.Exists(saveDir) || !Directory.Exists(saveNumDir))
        {
            GetControl<Text>("FileNotExit" + num.ToString()).gameObject.SetActive(true);
            GetControl<Image>("FileExit" + num.ToString()).gameObject.SetActive(false);
        }
        if (File.Exists(playerSave))
        {
            GetControl<Text>("FileNotExit" + num.ToString()).gameObject.SetActive(false);
            GetControl<Image>("FileExit" + num.ToString()).gameObject.SetActive(true);
            string data = File.ReadAllText(playerSave);
            Player playerInfo = JsonConvert.DeserializeObject<Player>(data);
            GetControl<Text>("GeoTxt").text = playerInfo.Money.ToString();
            int maxHp = playerInfo.MaxHp;
            UIMgr.Instance.CreatChildren("LifeMaskUI", lifeGridFile[num - 1].gameObject, maxHp);
        }
        else
        {
            GetControl<Text>("FileNotExit" + num.ToString()).gameObject.SetActive(true);
            GetControl<Image>("FileExit" + num.ToString()).gameObject.SetActive(false);
        }
        return playerSave;
    }

    public override void CheckKeyDown(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.Up.CurrentKey)
        {
            if (index > 0 && index < 5)
                index--;
            else if (index == 0)
            {
                index = 4;
            }
        }
        else if (key == KeyCodeMgr.Instance.Down.CurrentKey)
        {
            if (index == 4)
            {
                index = 0;
            }
            else if (index > 0 && index < 5)
            {
                index++;
            }
        }
        else if (key == KeyCodeMgr.Instance.Left.CurrentKey)
        {
            if (index >= 5 && index <= maxIndex)
            {
                index -= 5;
            }
        }
        else if (key == KeyCodeMgr.Instance.Right.CurrentKey)
        {
            if (index >= 0 && index < 4)
            {
                index += 5;
            }
        }
        else if (key == KeyCode.Space)
        {
            menuButtons[index].animator.SetTrigger("Pressed");
            ButtonPress();
        }
    }

    public override void ButtonPress()
    {
        switch (index)
        {
            case (int)FileChoiceMenuButton.FileOne:
                UIMgr.Instance.PopPanel();
                UIMgr.Instance.PopPanel();
                GameManager.Instance.ClearWhenChangeScene();
                InputMgr.Instance.StartOrEndCheck(false);
                ScenesMgr.Instance.LoadScene("SampleScene", () => {
                    //核心数据初始化（内涵各个管理器初始化和角色生成）
                    GameDataMgr.Instance.Init(fileOnePath);
                    //显示主面板
                    UIMgr.Instance.ShowPanel<BasePanel>("MainPanel", E_UI_Layer.Bot);
                    InputMgr.Instance.StartOrEndCheck(true);
                    MusicMgr.Instance.PlayBGMusic("MainStartPanel_BGM");
                });
                break;
            case (int)FileChoiceMenuButton.FileTwo:
                Debug.Log(fileTwoPath);
                break;
            case (int)FileChoiceMenuButton.FileThree:
                Debug.Log(fileThreePath);
                break;
            case (int)FileChoiceMenuButton.FileFour:
                Debug.Log(fileFourPath);
                break;
            case (int)FileChoiceMenuButton.Back:
                StartCoroutine(BackButton());
                break;
            case (int)FileChoiceMenuButton.ClearOne:
                ClearFile(fileOnePath, 1);
                break;
            case (int)FileChoiceMenuButton.ClearTwo:
                ClearFile(fileTwoPath, 2);
                break;
            case (int)FileChoiceMenuButton.ClearThree:
                ClearFile(fileThreePath, 3);
                break;
            case (int)FileChoiceMenuButton.ClearFour:
                ClearFile(fileFourPath, 4);
                break;
        }
    }

    IEnumerator BackButton()
    {
        anim.Play("FileChoicePanelFadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.PopPanel();
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    private void ClearFile(string path,int num)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("存档已删除");
            CheckSaveFiles(num);
        }
        else
        {
            Debug.Log("没存档");
        }
    }
}
