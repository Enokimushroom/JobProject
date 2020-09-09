using UnityEditor;
using System.IO;
using System.Diagnostics;


/*
    1.编译器类：继承自Editor类，只需要在unity编译器中执行的代码
    2.菜单项 特性[MenuItem("...")]:用于修饰产生菜单按钮的地址
    3.AssetDatabase:包含了只适用于编译器中操作资源的相关功能
    4.StreamingAssets：Unity特殊目录之一，目录中的文件不会被压缩，适合移动端读取资源。
      该Application.persistentDataPath路径可以在运行时进行读写操作，Unity外部目录。
 */

/// <summary>
/// 生成配置类文件
/// </summary>
public class GenerateResConfig : Editor
{
    [MenuItem("Tools/Resources/Generate ResConfig File")]
    public static void Generate()
    {
        //生成资源配置文件
        //1.查找resources目录下所有预制体的完整路径
        string[] resPrefabFiles = AssetDatabase.FindAssets("t:prefab t:Sprite t:TextAsset t:ScriptableObject t:AudioClip", new string[] {"Assets/Resources" });


        //GUID
        for (int i = 0; i < resPrefabFiles.Length; ++i)
        {
            resPrefabFiles[i] = AssetDatabase.GUIDToAssetPath(resPrefabFiles[i]);
            //2.生成对应关系
            //  名称=路径
            string fileName = Path.GetFileNameWithoutExtension(resPrefabFiles[i]);
            string filePath = resPrefabFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty).Replace(".psd", string.Empty).Replace(".txt", string.Empty).Replace(".asset", string.Empty).Replace(".wav", string.Empty).Replace(".png", string.Empty).Replace(".json", string.Empty);
            resPrefabFiles[i] = fileName + "=" + filePath;

        }
        //3.写入文件
        File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", resPrefabFiles);

    }
}
