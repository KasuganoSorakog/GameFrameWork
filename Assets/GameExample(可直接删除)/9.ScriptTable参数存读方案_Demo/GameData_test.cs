/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    //武器信息
    [Serializable]
    public struct weapon
    {
        [Header("武器名称")]
        public string name;

        [Header("每次升级所需要的金币数量"), Range(10, 10000)]
        public int[] Level_Pricel;
    }

    //站台
    [Serializable]
    public struct platform
    {
        [Header("每级站台的存储量")]
        public int[] Level_Capacity;
    }

    //火车信息
    [Serializable]
    public struct trainInformation
    {
        [Header("等待时间")]
        public int WaitTime;

        [Header("火车等级")]
        public int trainLevel;

        [Header("金币基础获取量")]
        public int GetGold_Base;
    }

    //宝箱信息
    [Serializable]
    public struct Treasure
    {
        public int hp;
        public int gold;
    }

    //资源价格
    [Serializable]
    public struct ResourcePrices
    {
        [Header("每金币兑换数(树木)")]
        public int wood_Price_ExchangeRate;
        [Header("每金币兑换数(石头)")]
        public int stone_Price_ExchangeRate;
        [Header("每金币兑换数(铁矿)")]
        public int Iron_Ore_Price_ExchangeRate;
        [Header("每金币兑换数(煤矿)")]
        public int Coal_Mine_Price_ExchangeRate;

    }

    public class GameData_test : ScriptableObject
    {
        //保存武器信息 (数组代表每一种武器的数据)
        [Header("武器数据")]
        public weapon[] weaponsData;

        //站台存储信息 (数组代表每一个站台的数据)
        [Header("站台数据")]
        public platform[] platformData;

        //火车运输 (数组代表每一列火车的数据)
        [Header("火车数据")]
        public trainInformation[] trainInformationData;

        [Header("火车升级的时候增加的基础获取量")]
        public int[] trainUp_AddGetGold;

        //宝箱信息
        [Header("宝箱数据")]
        public Treasure[] treasures;

        //资源贩卖
        [Header("资源数据")]
        public ResourcePrices gameResouceSell;
    }
}
