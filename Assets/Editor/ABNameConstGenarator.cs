/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2022-03-31 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class ABNameConstGenarator 
{
    [MenuItem("Tools/核心工具/AB包名转常量",false, 1055)]
    public static void GetABNameEnum()
    {
        //获取所有AB包名字
        string[] tags = AssetDatabase.GetAllAssetBundleNames();
        string arg = "";
        foreach (var tag in tags)
        {
            string temp = tag.Replace(".", "_");
            arg += "\t" + "public const string " + temp + " = " + "\"" + tag + "\"" + ";\n";
        }
        var tips = "//将Unity中所有Tag变成常量 \n";
        var res = tips + "public class ABType\n{\n" + arg + "}\n";
        var path = Application.dataPath + "/Scripts/Data(Model)/ABType.cs";
        File.WriteAllText(path, res, Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
