using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="new Level",menuName = "LevelMgr/new Level")]
public class LevelInfo : ScriptableObject
{
    public bool isDungeon = true;
    [System.Serializable]
    public class LevelBase
    {
        public EnermyPos[] enermyArray;
        public UnityEvent onStart;
        public UnityEvent onFinish;
    }
    public LevelBase[] levelInfo;
}

[System.Serializable]
public class EnermyPos
{
    public string enermyName;
    public Vector2 enermyPos;
    public GenerateType enermyType;
}

public enum GenerateType
{
    SmallBox,
    BigBox,
    None
}