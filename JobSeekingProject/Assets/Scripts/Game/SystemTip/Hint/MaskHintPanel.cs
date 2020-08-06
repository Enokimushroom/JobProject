using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaskHintPanel : BasePanel
{
    private void ShowOff()
    {
        transform.GetComponent<Animator>().SetTrigger("Off");
    }

    public void PopPanel()
    {
        UIMgr.Instance.PopPanel();
    }

    public void TurnOnAnim(int num)
    {
        transform.GetComponent<Animator>().SetInteger("MaskNum", num);
        Invoke("ShowOff", 3.0f);
    }
}
