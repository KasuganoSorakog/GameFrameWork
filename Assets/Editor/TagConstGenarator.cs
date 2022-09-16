/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-10-21 
 *说明:    一键将Tag转化成一个个枚举型，需要使用Tag时候不需要再输入String啦
 *         直接使用 EmTag. 即可点出想要的Tag
**/  
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class TagConstGenarator
{
    [MenuItem("Tools/核心工具/Tag转常量",false,1050)]
    public static void GenTagEnum()
    {
        var tags = InternalEditorUtility.tags;
        var arg = "";
        foreach (var tag in tags)
        {
            arg += "\t" + "public const string " + tag + " = " + "\"" + tag + "\""  + ";\n";
        }
        var tips = "//将Unity中的所有Tag变成常量\n";
        var res = tips + "public class TagType\n{\n" + arg + "}\n";
        var path = Application.dataPath + "/Scripts/Data(Model)/TagType.cs";
        File.WriteAllText(path, res, Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("成功将所有Tag转变成了常量！目录：Scripts/Data(Model)/TagType.cs 使用示例：TagType.Player == \"Player\"");
    }
}
