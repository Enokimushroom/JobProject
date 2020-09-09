using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogOptionPanel : BasePanel
{
    public override void ShowMe()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 600);
    }

    public Text GetDialogQuestionText()
    {
        return GetControl<Text>("QuestionTxt");
    }

    public GameObject[] GetDialogButton()
    {
        GameObject[] button = new GameObject[2];
        button[0] = GetControl<Button>("leftButton").gameObject;
        button[1] = GetControl<Button>("rightButton").gameObject;
        return button;
    }
}
