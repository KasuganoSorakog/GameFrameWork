/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2022-03-24 
 *说明:    打包APK后，在手机上显示错误信息
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ErrorDisplay : MonoBehaviour
{
    internal void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    internal void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    private string logs;
    /// <summary>
    ///
    /// </summary>
    /// <param name="logString">错误信息</param>
    /// <param name="stackTrace">跟踪堆栈bai</param>
    /// <param name="type">错误类型</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logs += logString + "\n";
        errMsg.text = logs;
    }
    public Text errMsg;
    public bool Log;
    //private Vector2 scroll;
    //internal void OnGUI()
    //{
    //    if (!Log)
    //        return;
    //    scroll = GUILayout.BeginScrollView(scroll);
    //    GUILayout.Label(m_logs);
    //    GUILayout.EndScrollView();
    //}
}

