using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class ExcelToJson : EditorWindow
{
    private static ExcelToJson instance;
    private static bool keepSource = true;
    private static string pathRoot;
    private static List<string> excelList;
    private static Vector2 scrollPos;

    ExcelToJson() { this.titleContent = new GUIContent("一键Excel转Json");}

    [MenuItem("Tools/Resources/ExcelToJson")]
    private static void ExcelToJsonWindow()
    {
        Rect re = new Rect(0, 0, 250, 175);
        GetWindowWithRect(typeof(ExcelToJson), re, true);
        Init();
        LoadExcel();
        instance.Show();
    }

    private void OnSelectionChange()
    {
        Show();
        LoadExcel();
        Repaint();
    }

    private void OnGUI()
    {
        keepSource = GUILayout.Toggle(keepSource, "保留Excel源文件");
        if (excelList == null) return;
        if (excelList.Count < 1) EditorGUILayout.LabelField("没有Excel文件被选中.");
        else
        {
            EditorGUILayout.LabelField("下列项目将转换为Json:");
            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(100));
            foreach(string s in excelList)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Toggle(true, s);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            if (GUILayout.Button("Confirm"))
            {
                Convert();
            }
        }
    }

    private static void Convert()
    {
        foreach (string assetsPath in excelList)
        {
            string excelPath = pathRoot + "/" + assetsPath;
            ExcelUtility excel = new ExcelUtility(excelPath);

            //判断输出类型
            string outputPath = excelPath.Replace(".xlsx", ".json");
            excel.ConvertToJson(outputPath, Encoding.GetEncoding("utf-8"));

            //是否保留源文件
            if (!keepSource)
            {
                FileUtil.DeleteFileOrDirectory(excelPath);
            }

            AssetDatabase.Refresh();
        }
        //转换后关闭
        instance.Close();
    }

    private static void Init()
    {
        instance = GetWindow<ExcelToJson>();
        pathRoot = Application.dataPath;
        pathRoot = pathRoot.Substring(0, pathRoot.LastIndexOf("/"));
        excelList = new List<string>();
        scrollPos = new Vector2(instance.position.x, instance.position.y + 75);
    }

    private static void LoadExcel()
    {
        if (excelList == null) excelList = new List<string>();
        excelList.Clear();
        object[] selection = (object[])Selection.objects;
        if (selection.Length == 0) return;
        //遍历每个对象是不是Excel文件
        foreach(Object obj in selection)
        {
            string objPath = AssetDatabase.GetAssetPath(obj);
            if (objPath.EndsWith(".xlsx"))
                excelList.Add(objPath);
        }
    }
}
