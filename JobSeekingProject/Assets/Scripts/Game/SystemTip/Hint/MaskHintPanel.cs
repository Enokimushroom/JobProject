using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaskHintPanel : BasePanel
{
    private void ShowOff()
    {
        transform.GetComponent<Animator>().SetTrigger("Off");
        PlayerStatus.Instance.IsForzen = false;
    }

    public void PopPanel()
    {
        UIMgr.Instance.PopPanel();
    }

    public void TurnOnAnim(int num)
    {
        transform.GetComponent<Animator>().SetInteger("MaskNum", num);
        Invoke("ShowOff", 3.0f);
        if (num == 4)
        {
            GameDataMgr.Instance.playerInfo.hideList.Find(x => x.id == 62).num -= 4;
            GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.最大血量, 1);
        }
    }

}
