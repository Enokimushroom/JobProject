using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Dialog",menuName = "Dialogue/Dialog")]
public class DialogBase : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        //public string speakerName;
        //public Sprite protrait;
        [TextArea(4, 8)]
        public string contentTxt;
    }

    public Info[] dialogInfo;
}
