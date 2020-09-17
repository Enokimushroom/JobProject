using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

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
    public bool isLocked { get; set; }

    public override void ShowMe()
    {
        base.ShowMe();
        anim = GetComponent<Animator>();

        fileOnePath = CheckSaveFiles(1);
        fileTwoPath = CheckSaveFiles(2);
        fileThreePath = CheckSaveFiles(3);
        fileFourPath = CheckSaveFiles(4);
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
        isLocked = false;
    }

    private string CheckSaveFiles(int num)
    {
        string saveDir = GameDataMgr.Instance.PlayerInfo_Url;
        string saveNumDir = saveDir + "/save" + num.ToString();
        string playerSave = saveNumDir + "/playerInfo.txt";
        if (!Directory.Exists(saveDir) || !Directory.Exists(saveNumDir))
        {
            Directory.CreateDirectory(saveNumDir);
            GetControl<Text>("FileNotExit" + num.ToString()).gameObject.SetActive(true);
            GetControl<Image>("FileExit" + num.ToString()).gameObject.SetActive(false);
        }
        if (File.Exists(playerSave))
        {
            GetControl<Text>("FileNotExit" + num.ToString()).gameObject.SetActive(false);
            GetControl<Image>("FileExit" + num.ToString()).gameObject.SetActive(true);
            string data = File.ReadAllText(playerSave);
            Player playerInfo = JsonConvert.DeserializeObject<Player>(data);
            int maxHp = playerInfo.MaxHp;
            GetControl<Text>("GeoTxt" + num.ToString()).text = playerInfo.Money.ToString();
            UIMgr.Instance.CreatChildren("LifeMaskUI", lifeGridFile[num - 1].gameObject, maxHp);
            GetControl<Image>("LocalImg" + num.ToString()).sprite = ResMgr.Instance.Load<Sprite>("Area_" + playerInfo.MapType.ToString());
            GetControl<Text>("LocalTxt" + num.ToString()).text = playerInfo.MapType.ToString();
            int min = (int)(playerInfo.playTime / 60);
            int hour = min / 60;
            GetControl<Text>("TimeTxt" + num.ToString()).text = hour.ToString() + "H" + min.ToString() + "M";
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
            else if (index >= 0 && index < 5)
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
                GameStart(fileOnePath);
                break;
            case (int)FileChoiceMenuButton.FileTwo:
                GameStart(fileTwoPath);
                break;
            case (int)FileChoiceMenuButton.FileThree:
                GameStart(fileThreePath);
                break;
            case (int)FileChoiceMenuButton.FileFour:
                GameStart(fileFourPath);
                break;
            case (int)FileChoiceMenuButton.Back:
                StartCoroutine(BackButton());
                break;
            case (int)FileChoiceMenuButton.ClearOne:
                ConfirmClear(fileOnePath, 1);
                break;
            case (int)FileChoiceMenuButton.ClearTwo:
                ConfirmClear(fileTwoPath, 2);
                break;
            case (int)FileChoiceMenuButton.ClearThree:
                ConfirmClear(fileThreePath, 3);
                break;
            case (int)FileChoiceMenuButton.ClearFour:
                ConfirmClear(fileFourPath, 4);
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

    public override void OnPause()
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
        isLocked = true;
    }

    public override void OnResume()
    {
        Invoke("Buffer", 0.25f);
    }

    private void ConfirmClear(string path, int num)
    {
        if (File.Exists(path))
        {
            UIMgr.Instance.ShowConfirmPanel("是否真的删除存档?", ConfirmType.TwoBtn, () => { ClearFile(path, num); });
        }
        else
        {
            Debug.Log("没存档");
        }
    }

    private void ClearFile(string path,int num)
    {
        File.Delete(path);
        Debug.Log("存档已删除");
        CheckSaveFiles(num);
    }

    private void GameStart(string path)
    {
        UIMgr.Instance.ClearPanelStack();
        InputMgr.Instance.StartOrEndCheck(false);
        MusicMgr.Instance.StopBGMusic();
        GameManager.Instance.TimePause = false;
        ScenesMgr.Instance.goingScene = false;
        PlayerStatus.Instance.IsAlive = false;
        GameDataMgr.Instance.Init(path);
    }

    /// <summary>
    /// 防止粘键
    /// </summary>
    private void Buffer()
    {
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
        isLocked = false;
    }
}
