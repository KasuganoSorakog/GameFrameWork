/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-17 
 *说明:    自动生成UIPanel的常量信息    路径: Scripts/Data(Model)/UIPanelTypes.cs
 *         自动生成UIPanel的ScriptTable 路径: Assets/Resources/Data/UIPanelPathData.asset
**/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScriptTableGenerator 
{   
    [MenuItem("Tools/核心工具/UpDataUIPanel 自动更新Panel信息",false,1050)]
    static void GetUIResouceAll()
    {
        GameObject[] AllPanel = Resources.LoadAll<GameObject>("UIPanel");
        string[] panelName = new string[AllPanel.Length];
        string[] panelpaths = new string[AllPanel.Length];
        //Debug.Log(AllPanel.Length + "dddd :" + AllPanel.Length.IsZero());
        if (AllPanel.Length == 0)
        {
            Debug.LogError("找不到Resources/UIPanel下的资源！");
            return;
        }
        for (int i = 0; i < AllPanel.Length; i++)
        {
            panelName[i] = AllPanel[i].name;
            panelpaths[i] = "UIPanel/" + AllPanel[i].name;          
        }

        //1. 自动化创建PanelType文件(包含更新)
        CreatePanelType(AllPanel);
        //2. 自动化创建包含UI路径信息的ScriptTable(包含更新) TODO:
        CreatePanelPathData(AllPanel.Length, AllPanel, panelName, panelpaths);
        //3. 获取所有的Panel脚本
        CreateScriptAndHang(AllPanel);
        //4. 自动生成所有的PanelData脚本
        //CreatePanelData(AllPanel);
        //5. 删除一些垃圾脚本
        DeleteOverScript(AllPanel);
        Debug.Log("数据更新完成！");
    }

    //自动化创建PanelType文件
    static void CreatePanelType(GameObject[] AllPanel)
    {
        string arg = "";
        string tips = "//说明:UI窗口的类型，可以直接通过类名访问，转换字符串为常量，直接调用" + "\n";
        foreach (var panel in AllPanel)
        {
            arg += "\t" + "public const string " + panel.name + " =" + "\"" + panel.name + "\"" + ";\n";
        }

        string res = tips + "public class UIPanelType\n{\n" + arg + "}\n";
        string path = Application.dataPath + "/Scripts/Data(Model)/UIPanelType.cs";
        File.WriteAllText(path, res, Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("已更新UIPanelType的Panel面板信息！");
    }

    //自动化创建包含UI路径信息的ScriptTable
    static void CreatePanelPathData(int count, GameObject[] panelPre, string[] panelName, string[] panelPath)
    {
        string path = "Assets/Resources/Data/UIPanelPathData.asset";
        UIPanelPathData uipathData = ScriptableObject.CreateInstance<UIPanelPathData>();
        uipathData.panelData = new PanelData[count];
        for (int i = 0; i < count; i++)
        {
            uipathData.panelData[i].panelPre = panelPre[i];
            uipathData.panelData[i].panelName = panelName[i];
            uipathData.panelData[i].path = panelPath[i];
        }
        AssetDatabase.CreateAsset(uipathData, path);
        AssetDatabase.Refresh();
        Debug.Log("已更新UIPanelPathData的路径信息！");
    }

    //自动化生成脚本，并挂载物体上去
    static void CreateScriptAndHang(GameObject[] AllPanel)
    {
        //获取Script/Main/UIFramework文件夹下所有UIPanel脚本的名字
        string path = Application.dataPath + "/Scripts/Entity(View)/UIFramework/UIPanel/";
        List<string> panelNameList = new List<string>();
        List<GameObject> unUsedPanel = new List<GameObject>();

        //获取指定路径下面的所有资源文件, 并找出有哪些Panel没有挂脚本的,放入unUsedPanel中
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                panelNameList.Add(files[i].Name.Split('.')[0]);
            }
        }
        else
        {
            Debug.Log(path + "为空！");
            return;
        }

        for (int i = 0; i < AllPanel.Length; i++)
        {
            if (!panelNameList.Exists(t => t.Equals(AllPanel[i].name)))
            {
                unUsedPanel.Add(AllPanel[i]);
            }
        }

        //根据检测到的未挂脚本的窗口，生成对应的脚本内容并挂上去
        for (int i = 0; i < unUsedPanel.Count; i++)
        {
            //新脚本的路径
            string n_path = path + unUsedPanel[i].name + ".cs";
            //1.脚本提示
            string tips = "//该脚本为自动生成的Panel脚本\n";
            string usingArg = "using UnityEngine;\nusing UnityEngine.UI;\n";
            //2.脚本开头内容
            string res = "public class " + unUsedPanel[i].name + " : BasePanel{" + "\n";
            string arg_1 = "";
            string arg_2 = "\n\n\tpublic override void Awake()\n\t{\n\t\tbase.Awake();\n\t}\n";
            string arg_3 = "";
            string arg_4 = "\n\n\tpublic void OnDestroy()\n\t{\n\t}\n";
            string arg_5 = "\tpublic override void OnEnter()  { base.OnEnter();} \n\n" +
                           "\tpublic override void OnPause()  { base.OnPause();} \n\n" +
                           "\tpublic override void OnResume() { base.OnResume();}\n\n" +
                           "\tpublic override void OnExit()   { base.OnExit();}\n\n";
            //string arg_6 = "\tpublic " + unUsedPanel[i].name + "(" + unUsedPanel[i].name + "Data " + unUsedPanel[i].name + "Data " + " = null)\n\t{\n\t\tthis." + unUsedPanel[i].name + "Data = " + unUsedPanel[i].name + "Data;\n\t}";
            //string arg_7 = "\tprivate " + unUsedPanel[i].name + "Data " + unUsedPanel[i].name + "Data;\n";
            string arg_8 = "\tprivate DataManager dataManager{ get { return DataManager.Instance; } }\n";

            //string arg_6 = "\n\t public override void SetPanelData(UIPanelData uiPanelData)\n\t {\n\t\t" + unUsedPanel[i].name + "Data =" + " (" + unUsedPanel[i].name + "Data)" + "uiPanelData;\n\t }\n";
            //3.获取Panel内所有的Button，对应的生成事件
            Button[] btns = unUsedPanel[i].GetComponentsInChildren<Button>();

            if (btns.Length >0)
            {
                foreach (var btn in btns)
                {
                    arg_1 += "\n\t" + "public Button " + btn.name + " ;\n\n";
                    arg_2  = arg_2.Split('}')[0] + "\n\t\t"+ btn.name + ".onClick.AddListener(On" + btn.name + "_OnClick);\n\t}";
                    arg_3 += "\tprivate void On" + btn.name + "_OnClick()" + "{}\n\n";
                    arg_4 = arg_4.Split('}')[0] + "\n\t\t" + btn.name + ".onClick.RemoveListener(On" + btn.name + "_OnClick);\n\t}\n\n";
                }
                //整合以上所有代码  (+ arg_6)
                //res = tips + usingArg + res + "\n" + arg_7 + arg_1  + arg_2 + arg_4 + arg_5 + arg_3  + arg_6 + "}\n";
                res = tips + usingArg + res + arg_8 + "\n" + arg_1 + arg_2 + arg_4 + arg_5 + arg_3 + "}\n";
                File.WriteAllText(n_path, res, Encoding.UTF8);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                unUsedPanel[i].AddComponent<CanvasGroup>();               
            }
            else
            {
                //整合以上所有代码
                //res = tips + usingArg + res + "\n" + arg_7 + arg_1  + arg_2 + "\n"+  arg_5 + arg_3 + arg_6 + "}\n";
                res = tips + usingArg + res + arg_8 + "\n" + arg_1 + arg_2 + "\n" + arg_5 + arg_3 + "}\n";
                File.WriteAllText(n_path, res, Encoding.UTF8);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                unUsedPanel[i].AddComponent<CanvasGroup>();
            }
            Debug.LogWarning("成功给" + unUsedPanel[i].name + "生成Panel脚本，请手动挂上去！");
        }
    }

    //自动生成对应的数据文件(暂不启用，所有数据从DataManager中获取)
    static void CreatePanelData(GameObject[] AllPanel)
    {
        //获取当前文件目录下的所有UI数据文件
        string path = Application.dataPath + "/Scripts/Entity(View)/UIFramework/UIData/";
        List<string> uiPanelData = new List<string>();
        //缺失Data的Panel
        List<GameObject> unDataPanel = new List<GameObject>();
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                uiPanelData.Add(files[i].Name.Split('.')[0]);
            }
        }
        else
        {
            Debug.Log(path + "目录为空！");
        }

        //计算出有多少个Panel没有数据文件
        for (int i = 0; i < AllPanel.Length; i++)
        {
            if (!uiPanelData.Exists(t => t.Equals(AllPanel[i].name + "Data")))
            {
                unDataPanel.Add(AllPanel[i]);
            }
        }

        //如果这些窗口没有数据文件,则新生成一份
        for (int i = 0; i < unDataPanel.Count; i++)
        {
            //新脚本的路径
            string n_path = path + unDataPanel[i].name + "Data.cs";
            string tips = "//该脚本为自动生成的UIData脚本 \n";
            string res = tips + "public class " + unDataPanel[i].name + "Data : UIPanelData{\n}";
            File.WriteAllText(n_path, res, Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("成功给" + unDataPanel[i].name + "生成数据脚本！");
        }
    }

    //检测是否有多余的脚本，执行删除
    static void DeleteOverScript(GameObject[] AllPanel)
    {

        //获取Script/Main/UIFramework文件夹下所有UIPanel脚本的名字
        string path_UIpanel_Script = Application.dataPath + "/Scripts/Entity(View)/UIFramework/UIPanel/";
        string path_UIDatapanel_Script = Application.dataPath + "/Scripts/Entity(View)/UIFramework/UIData/";

        List<string> panelScriptNameList = new List<string>();
        List<string> panelDataScriptNameList = new List<string>();
        List<string> panelList = new List<string>();

        //未使用的Script脚本
        List<string> unUsedPanel_Script = new List<string>();
        List<string> unUsedPanelData_Script = new List<string>();

        //获取所有UIPanel的脚本文件
        if (Directory.Exists(path_UIpanel_Script))
        {
            DirectoryInfo direction = new DirectoryInfo(path_UIpanel_Script);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                panelScriptNameList.Add(files[i].Name.Split('.')[0]);
            }
        }
        else
        {
            Debug.Log(path_UIpanel_Script + "为空！");
            return;
        }
        //--------------------------------------------------
        if (Directory.Exists(path_UIDatapanel_Script))
        {
            DirectoryInfo direction = new DirectoryInfo(path_UIDatapanel_Script);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                panelDataScriptNameList.Add(files[i].Name.Split('.')[0]);
            }
        }
        else
        {
            Debug.Log(path_UIDatapanel_Script + "为空！");
            return;
        }
        //---------------------------------------------------
        for (int i = 0; i < AllPanel.Length; i++)
        {
            panelList.Add(AllPanel[i].name);
        }
        //---------------------------------------------------
        for (int i = 0; i < panelScriptNameList.Count; i++)
        {
            if (!panelList.Exists(t => t.Equals(panelScriptNameList[i])))
            {
                string file_path = path_UIpanel_Script + panelScriptNameList[i] + ".cs";
                string file_meta_path = path_UIpanel_Script + panelScriptNameList[i] + ".cs.meta";
                File.Delete(file_meta_path);
                File.Delete(file_path);         
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("当前发现" + panelScriptNameList[i] + ".cs是多余的, 已经进行清理" );
            }
        }
        //---------------------------------------------------
        for (int i = 0; i < panelDataScriptNameList.Count; i++)
        {
            if (panelDataScriptNameList[i].Equals("UIPanelData"))
            {
                continue;
            }
            if (!panelList.Exists(t => t.Equals(panelDataScriptNameList[i].Split(new string[] { "Data" }, StringSplitOptions.RemoveEmptyEntries)[0])))
            {
                string file_path = path_UIDatapanel_Script + panelDataScriptNameList[i] + ".cs";
                string file_meta_path = path_UIDatapanel_Script + panelDataScriptNameList[i] + ".cs.meta";
                File.Delete(file_meta_path);
                File.Delete(file_path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("当前发现" + panelDataScriptNameList[i] + ".cs是多余的, 已经进行清理");
            }
        }      
    }


}


