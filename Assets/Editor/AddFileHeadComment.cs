/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    用于创建头部注释的文件
 * 注意：  此脚本需要到Unity安装目录 D:\Unity 2019.4.16f\Editor\Data\Resources\ScriptTemplates 修改 81-C# Script-NewBehaviourScript.cs.txt才可生效
 * 修改内容在脚本的最下方，取消注释后，复制粘贴保存即可生效
**/

using UnityEditor;
using UnityEngine;
using System.IO;

public class AddFileHeadComment : UnityEditor.AssetModificationProcessor  
{ 
    /// <summary>  
    /// 此函数在asset被创建完，文件已经生成到磁盘上，但是没有生成.meta文件和import之前被调用  
    /// </summary>  
    /// <param name="newFileMeta">newfilemeta 是由创建文件的path加上.meta组成的</param>  
    public static void OnWillCreateAsset(string newFileMeta)
    { 
        string newFilePath = newFileMeta.Replace(".meta", "");
        string fileExt = Path.GetExtension(newFilePath);  
        if (fileExt != ".cs")  
        { 
            return;  
        }  
        //注意，Application.datapath会根据使用平台不同而不同  
        string realPath = Application.dataPath.Replace("Assets", "") + newFilePath;
        string scriptContent = File.ReadAllText(realPath);
        //这里实现自定义的一些规则  
        scriptContent = scriptContent.Replace("#AUTHOR#", "  Sora");
        scriptContent = scriptContent.Replace("#COMPANY#", PlayerSettings.companyName);
        scriptContent = scriptContent.Replace("#UNITYVERSION#", "   "+Application.unityVersion);  
        scriptContent = scriptContent.Replace("#DATE#", System.DateTime.Now.ToString("yyyy-MM-dd"));  
        File.WriteAllText(realPath, scriptContent);  
    }


//--------------------------------------------------
//    /** 
// *Copyright(C) 2021 by #COMPANY# 
// *All rights reserved. 
// *作者:       #AUTHOR# 
// *Unity版本：#UNITYVERSION# 
// *日期:         #DATE# 
// *说明:    
//**/  
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class #SCRIPTNAME# : MonoBehaviour
//{

//    void Start()
//{
//# NOTRIM#
//}

//void Update()
//{
//# NOTRIM#
//}
//}
//------------------------------------------------
}  