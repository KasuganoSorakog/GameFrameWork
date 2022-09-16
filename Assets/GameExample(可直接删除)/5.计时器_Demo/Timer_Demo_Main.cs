/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-25 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class Timer_Demo_Main : MonoBehaviour
    {
        GameTimer timer = new GameTimer();

        private void Start()
        {
            timer.Set(5);
        }

        private void Update()
        {
            if (timer.GetEnable() && timer.IsTimeOut())
            {
                Debug.Log("5秒钟到啦！");
                timer.SetEnable(false);

                // timer.SetDoOnce(false); 循环计时，当时间到了后自动清0,重新计数
                // timer.SetPause(true);   计时器暂停
            }
        }
    }

}
