using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public override void ShowMe()
    {
        base.ShowMe();
        EventCenter.Instance.AddEventListener<int>("SceneLoadingProcess", SetLoadingInfo);
    }

    public void SetLoadingInfo(int index)
    {
        GetControl<Slider>("ProcessSlider").value = (float)index / 100;
        GetControl<Text>("LoadingTxt").text = index + "%";
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<int>("SceneLoadingProcess", SetLoadingInfo);
    }
}
