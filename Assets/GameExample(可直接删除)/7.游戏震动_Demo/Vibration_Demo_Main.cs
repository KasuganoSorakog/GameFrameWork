/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-25 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class Vibration_Demo_Main : MonoBehaviour
    {
        //在安卓机中生效， 苹果机未测试过
        void Start()
        {
            //轻度震动
            //VibrationManager.TriggerLight_Shock();

            //中度震动
            //VibrationManager.TriggerMid_Shock();

            //高度震动
            //VibrationManager.TriggerGrave_Shock();

        }
    }
}

