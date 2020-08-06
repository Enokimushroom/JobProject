using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanel : BasePanel
{
    public enum AudioMenuButton
    {
        Main,
        Music,
        Sound,
        Reset,
        Back,
    }

    [HideInInspector] public int index;
    [HideInInspector] public int maxIndex;
    [HideInInspector] public UIButton[] menuButtons;

    public override void ShowMe()
    {
        base.ShowMe();
    }
    private void Start()
    {
        GetControl<Slider>("MainSlider").onValueChanged.AddListener(ChangeMasterVolume);
        GetControl<Slider>("MusicSlider").onValueChanged.AddListener(ChangeMusicVolume);
        GetControl<Slider>("SoundSlider").onValueChanged.AddListener(ChangeSoundVolume);
        GetControl<Text>("MainVolumeTxt").text = (MusicMgr.Instance.mainValue * 10).ToString();
        GetControl<Text>("MusicVolumeTxt").text = (MusicMgr.Instance.bgValue * 10).ToString();
        GetControl<Text>("SoundVolumeTxt").text = (MusicMgr.Instance.soundValue * 10).ToString();
        menuButtons = GetComponentsInChildren<UIButton>();
        index = 0;
        maxIndex = menuButtons.Length;
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
    }

    private void ChangeMasterVolume(float value)
    {
        float volume = (10 - value) / 10;
        MusicMgr.Instance.ChangeMainValue(volume);
        GetControl<Text>("MainVolumeTxt").text = (volume * 10).ToString();
    }

    private void ChangeMusicVolume(float value)
    {
        float volume = (10 - value) / 10;
        MusicMgr.Instance.ChangeBGValue(volume);
        GetControl<Text>("MusicVolumeTxt").text = (volume * 10).ToString();
    }

    private void ChangeSoundVolume(float value)
    {
        float volume = (10 - value) / 10;
        MusicMgr.Instance.ChangeSoundValue(volume);
        GetControl<Text>("SoundVolumeTxt").text = (volume * 10).ToString();
    }

    private void CheckKeyDown(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.Up.CurrentKey)
        {
            if (index > 0)
                index--;
            else
                index = maxIndex;
        }
        else if (key == KeyCodeMgr.Instance.Down.CurrentKey)
        {
            if (index < maxIndex)
                index++;
            else
                index = 0;
        }
        else if (key == KeyCode.Space)
        {
            if (index == (int)AudioMenuButton.Reset || index == (int)AudioMenuButton.Back)
            {
                menuButtons[index].animator.SetTrigger("Pressed");
                ButtonPress();
            }
        }
        else if(key == KeyCodeMgr.Instance.Left.CurrentKey)
        {
            if (index == (int)AudioMenuButton.Reset || index == (int)AudioMenuButton.Back) return;
            menuButtons[index].GetComponentInChildren<Slider>().value += 1f;
        }
        else if (key == KeyCodeMgr.Instance.Right.CurrentKey)
        {
            if (index == (int)AudioMenuButton.Reset || index == (int)AudioMenuButton.Back) return;
            menuButtons[index].GetComponentInChildren<Slider>().value -= 1f;
        }

    }

    public void ButtonPress()
    {
        switch (index)
        {
            case (int)AudioMenuButton.Reset:
                ResetAudioVolume();
                break;
            case (int)AudioMenuButton.Back:
                StartCoroutine(BackButton());
                break;
        }
    }

    private void ResetAudioVolume()
    {
        GetControl<Slider>("MainSlider").value = 0;
        GetControl<Slider>("MusicSlider").value = 0;
        GetControl<Slider>("SoundSlider").value = 0;
    }

    IEnumerator BackButton()
    {
        GetComponent<Animator>().Play("MusicPanelFadeOut");
        yield return new WaitForSeconds(0.6f);
        UIMgr.Instance.PopPanel();
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
    }
}
