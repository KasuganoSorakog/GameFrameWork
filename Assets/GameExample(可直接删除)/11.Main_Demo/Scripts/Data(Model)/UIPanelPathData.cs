/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-17 
 *说明:    
**/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo 
{
    [Serializable]
    public struct PanelData
    {
        public GameObject panelPre;
        public string panelName;
        public string path;
    };

    [Serializable]
    public class UIPanelPathData : ScriptableObject
    {
        public PanelData[] panelData;
    }

}
