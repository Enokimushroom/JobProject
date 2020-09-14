using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanelInStartScene
{
    public enum PausePanelButton
    {
        Continue,
        Setting,
        BackToMainPanel,
    }

    public override void Start()
    {
        base.Start();
        //暂停时间以及时间计算
        GameManager.Instance.TimePause = true;
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void ShowMe()
    {
        base.ShowMe();
        PlayerStatus.Instance.InputEnable = false;
        PlayerStatus.Instance.IsForzen = true;
        GameManager.Instance.playerGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void HideMe()
    {
        base.HideMe();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void ButtonPress()
    {
        base.ButtonPress();
        switch (index)
        {
            case (int)PausePanelButton.Continue:
                StartCoroutine(ContinueButton());
                break;
            case (int)PausePanelButton.Setting:
                StartCoroutine(SettingButton());
                break;
            case (int)PausePanelButton.BackToMainPanel:
                StartCoroutine(BackButton());
                break;
        }
    }

    private IEnumerator ContinueButton()
    {
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.85f);
        GameManager.Instance.TimePause = false;
        PlayerStatus.Instance.InputEnable = true;
        PlayerStatus.Instance.IsForzen = false;
        UIMgr.Instance.PopPanel();
    }

    private IEnumerator SettingButton()
    {
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.85f);
        UIMgr.Instance.ShowPanel<BasePanel>("SettingPanel", E_UI_Layer.top);
    }

    private IEnumerator BackButton()
    {
        //自带一次保存
        GameManager.Instance.UpdateGameTime();
        GetComponent<Animator>().Play("FadeOut");
        GameManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.85f);
        UIMgr.Instance.ClearPanelStack();
        ScenesMgr.Instance.goingScene = true;
        ScenesMgr.Instance.LoadSceneAsyn("MainScene", ()=> {
            UIMgr.Instance.ShowPanel<BasePanel>("MainStartPanel", E_UI_Layer.Bot);
        });
    }

    public override void OnPause()
    {
        base.OnPause();
        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void OnResume()
    {
        base.OnResume();
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        GetComponent<Animator>().Play("FadeIn");
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }
}
