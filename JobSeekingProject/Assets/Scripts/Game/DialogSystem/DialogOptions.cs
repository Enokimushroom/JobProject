using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New DialogOption", menuName = "Dialogue/DialogOptions")]
public class DialogOptions : DialogBase
{
    [TextArea(2, 10)]
    public string questionText;

    [System.Serializable]
    public class Options
    {
        public string buttonName;
        public UnityEvent myEvent;
    }

    public Options[] optionsInfo;
}
