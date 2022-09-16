/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyDemo
{
    public class ScriptTableData_Demo_Editor
    {
        //该功能只在编辑器模式下使用，不参与游戏逻辑运行
        [MenuItem("Tools/TestDemo/创建游戏数据文件", false, 1050)]
        static void CreateScriptTable()
        {
            string path = "Assets/GameExample/9.ScriptTable参数存读方案_Demo/" + "gameData_test" + ".asset";

            GameData_test gameData_test = ScriptableObject.CreateInstance<GameData_test>();

            AssetDatabase.CreateAsset(gameData_test, path);
            AssetDatabase.Refresh();
            Debug.Log("已生成游戏数据信息！");
        }
    }

}
