using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class MainStartPanel : BasePanelInStartScene
{
    public enum MainMenuButton
    {
        Start,
        Setting,
        Extra,
        Quit,
    }

    public override void Start()
    {
        base.Start();
        MusicMgr.Instance.PlayBGMusic("MainStartPanel_BGM");
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void ButtonPress()
    {
        switch (index)
        {
            case (int)MainMenuButton.Start:
                Debug.Log("start");
                StartCoroutine(StartButton());
                break;
            case (int)MainMenuButton.Setting:
                StartCoroutine(SettingButton());
                break;
            case (int)MainMenuButton.Extra:
                
                break;
            case (int)MainMenuButton.Quit:
                Application.Quit();
                break;
        }
    }

    IEnumerator StartButton()
    {
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.ShowPanel<BasePanel>("FileChoicePanel", E_UI_Layer.Mid);
    }

    IEnumerator SettingButton()
    {
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.ShowPanel<BasePanel>("SettingPanel", E_UI_Layer.Mid);
    }

    public override void OnPause()
    {
        base.OnPause();
        GetComponent<CanvasGroup>().interactable = false;
        gameObject.SetActive(false);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
        GetComponent<Animator>().Play("FadeIn");
        GetComponent<CanvasGroup>().interactable = true;
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }
}
