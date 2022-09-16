/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-24 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class UIAnimation_Demo_Main : MonoBehaviour
    {
        //public UIManager m_uiManager;

        void Start()
        {
            //UI框架就是 Script/UIFrameWork/  所有文件, 拆出不影响
            //打开UI面板 - 无参写法
            //m_uiManager.ShowPanel(UIPanelType.MyPanel);

            //打开UI面板 - 有参写法
            //m_uiManager.ShowPanel(UIPanelType.MyPanel, new MyPanelData() { str = "我被点击了"});



        }

        void Update()
        {

        }
    }

}
