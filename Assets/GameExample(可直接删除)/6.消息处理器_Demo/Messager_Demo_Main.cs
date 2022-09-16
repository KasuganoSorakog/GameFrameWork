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
    public class Messager_Demo_Main : MonoBehaviour
    {

        private void Start()
        {
            //添加监听
            Messenger.AddListener("AA_Method", AA_Method);
            Messenger.AddListener<string>("BB_Method", BB_Method);

            //使用方法
            Messenger.Broadcast("AA_Method");
            Messenger.Broadcast("BB_Method", "str");
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener("AA_Method", AA_Method);
            Messenger.RemoveListener<string>("BB_Method", BB_Method);
        }

        public void AA_Method()
        {
            Debug.Log("我执行了AA的方法");
        }

        public void BB_Method(string str)
        {
            Debug.Log("我执行了BB的方法");
        }
    }

}
