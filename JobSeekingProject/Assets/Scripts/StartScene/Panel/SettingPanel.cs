using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanelInStartScene
{
    public enum SettingMenuButton
    {
        Music,
        Key,
        Back,
    }

    public override void Start()
    {
        base.Start();
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void ButtonPress()
    {
        switch (index)
        {
            case (int)SettingMenuButton.Music:
                StartCoroutine(MusicButton());
                break;
            case (int)SettingMenuButton.Key:
                StartCoroutine(KeyButton());
                break;
            case (int)SettingMenuButton.Back:
                StartCoroutine(BackButton());
                break;
        }
    }

    public override void OnPause()
    {
        GetComponent<Animator>().Play("SettingPanelFadeOut");
        base.OnPause();
        gameObject.SetActive(false);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
        GetComponent<Animator>().Play("SettingPanelFadeIn");
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    IEnumerator MusicButton()
    {
        GetComponent<Animator>().Play("SettingPanelFadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.ShowPanel<BasePanel>("MusicPanel", E_UI_Layer.top);
    }

    IEnumerator KeyButton()
    {
        GetComponent<Animator>().Play("SettingPanelFadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.ShowPanel<BasePanel>("KeySetPanel", E_UI_Layer.top);
    }

    IEnumerator BackButton()
    {
        GetComponent<Animator>().Play("SettingPanelFadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.PopPanel();
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }
}
