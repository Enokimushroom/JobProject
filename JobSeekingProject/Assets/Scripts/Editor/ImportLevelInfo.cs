using Boo.Lang;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ImportLevelInfo : EditorWindow
{
    string importID;

    ImportLevelInfo()
    {
        this.titleContent = new GUIContent("一键配置关卡怪物");
    }

    [MenuItem("Tools/Resources/ImportLevel")]
    static void ShowWindow()
    {
        //EditorWindow.GetWindow(typeof(testWindow)); --可变窗口大小
        //固定窗口大小
        Rect re = new Rect(0, 0, 250, 150);
        EditorWindow.GetWindowWithRect(typeof(ImportLevelInfo), re, true);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 20;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("输入目标关卡ID");

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        importID = EditorGUILayout.TextField("关卡ID", importID);

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUILayout.Label("当前场景:" + EditorSceneManager.GetActiveScene().name);

        EditorGUILayout.Space();

        if (GUILayout.Button("Confirm"))
        {
            ImportInfo();
        }
        if (GUILayout.Button("Quit"))
        {
            Close();
        }

        GUILayout.EndVertical();
    }

    private void ImportInfo()
    {
        if (importID != null)
        {
            LevelInfo lvInfo = ResMgr.Instance.Load<LevelInfo>(importID);
            if (lvInfo == null)
            {
                Debug.LogError("请输入正确的关卡ID");
                return;
            }
            GameObject[] enermyList = GameObject.FindGameObjectsWithTag("Enermy");
            List<EnermyPos> empList = new List<EnermyPos>();
            List<LevelInfo.LevelBase> tempList = new List<LevelInfo.LevelBase>();
            for (int i = 0; i < enermyList.Length; ++i)
            {
                EnermyStatus es = enermyList[i].GetComponentInChildren<EnermyStatus>();
                empList.Add(new EnermyPos
                {
                    enermyName = es.EnermyName,
                    enermyPos = es.transform.position,
                    enermyType = es.generateType
                });
            }
            tempList.Add(new LevelInfo.LevelBase { enermyArray = empList.ToArray() });
            lvInfo.levelInfo = tempList.ToArray();
        }
        else
        {
            Debug.LogError("请输入关卡ID!!");
        }
    }
}
