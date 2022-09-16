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
    public class DataSave_Demo_Main : MonoBehaviour
    {

        void Start()
        {
            //目前只能存读 int ,float , string， 可在GameSaveManager进行其他类型的拓展。
            //存的时候尽量不要用字符串，而在DataType里面添加常量，降低错误率
            GameSaveManager.Save(DataType.Level, 1);

            GameSaveManager.Load<int>(DataType.Level);

            //如何看数据？   菜单栏 Tools/PlayerPrefs查看器 可进行可视化查看
        }

    }

}
