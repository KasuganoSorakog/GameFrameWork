/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-29 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class GameDataSave_Demo
    {
        //玩家当前的权限等级
        public int player_Level;

        //玩家身上的金币
        public int player_Gold;

        //保存关卡的开启进度
        public List<bool> IsUnLockLevel;

        //保存玩家身上的资源，煤炭，矿物，金属等
        public Dictionary<string, string> GameRes;
    }
}

