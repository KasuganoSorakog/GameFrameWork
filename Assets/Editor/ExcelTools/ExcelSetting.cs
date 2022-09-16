using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Excel转json 路径数据
/// </summary>
public class ExcelSetting : ScriptableObject
{
    [SerializeField]
    [Header("json保存路径")]
    private string saveJsonPath = null;
    [SerializeField]
    [Header("json实体类保存路径")]
    private string saveCellPath = null;
    [SerializeField]
    [Header("实体类命名空间")]
    private string nameSpace = null;

    public string SaveJsonPath
    {
        get
        {
            return saveJsonPath;
        }
    }

    public string SaveCellPath
    {
        get
        {
            return saveCellPath;
        }
    }
    public string NameSpace
    {
        get
        {
            return nameSpace;
        }
    }

#if UNITY_EDITOR


    private static ExcelSetting m_Instance;

    public static ExcelSetting Instance
    {
        get
        {
            if (m_Instance == null)
            {
                string[] paths = AssetDatabase.FindAssets("t:ExcelSetting");
                if (paths.Length == 0)
                    throw new System.Exception("Not Find ExcelSetting");
                string path = AssetDatabase.GUIDToAssetPath(paths[0]);
                m_Instance = AssetDatabase.LoadAssetAtPath<ExcelSetting>(path);
            }
            return m_Instance;
        }
    }

#endif

    [MenuItem("Tools/CreateExcelSetting")]
    public static void CreateExcelSetting()
    {
        string[] paths = AssetDatabase.FindAssets("t:ExcelSetting");
        if (paths.Length >= 1)
        {
            string path = AssetDatabase.GUIDToAssetPath(paths[0]);
            EditorUtility.DisplayDialog("警告", $"已存在ExcelSetting，路径:{path}", "确认");
            return;
        }

        ExcelSetting setting = CreateInstance<ExcelSetting>();
        AssetDatabase.CreateAsset(setting, "Assets/ExcelSetting.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}