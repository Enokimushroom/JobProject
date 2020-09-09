using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItemHintPanel : BasePanel
{
    public ItemInfo item;

    public override void ShowMe()
    {
        if (item != null)
        {
            Item i = GameDataMgr.Instance.GetItemInfo(item.id);
            GetControl<Image>("HintImg").sprite = ResMgr.Instance.Load<Sprite>("SkillHint" + i.id.ToString());
            string key = null;
            switch (i.id)
            {
                case 5:
                    key = KeyCodeMgr.Instance.Recover.CurrentKey.ToString();
                    break;
                case 6:
                    key = KeyCodeMgr.Instance.Recover.CurrentKey.ToString();
                    break;
                case 8:
                    key = KeyCodeMgr.Instance.Recover.CurrentKey.ToString();
                    break;
                case 14:
                    key = KeyCodeMgr.Instance.Sprint.CurrentKey.ToString();
                    break;
                case 15:
                    key = KeyCodeMgr.Instance.Jump.CurrentKey.ToString();
                    break;
                case 16:
                    key = KeyCodeMgr.Instance.SuperSprint.CurrentKey.ToString();
                    break;
                case 17:
                    key = KeyCodeMgr.Instance.Jump.CurrentKey.ToString();
                    break;
            }
            GetControl<Text>("KeyTxt").text = key;
        }
        MusicMgr.Instance.PlaySound("UISkillHintAudio", false);
        Invoke("ShowOff", 3);

    }

    private void ShowOff()
    {
        transform.GetComponent<Animator>().SetTrigger("Off");

    }

    public void PopPanel()
    {
        UIMgr.Instance.PopPanel();
    }
}
