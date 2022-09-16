/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyDemo
{
    public class ScriptTableData_Demo_Main : MonoBehaviour
    {
        //获取游戏数据
        public GameData_test gameData_test;


        private void Start()
        {
            //Debug.Log(LoadScriptTable_Data_Weapon_Price("剑", 2));

            SaveScriptTable_Data_Weapon_Price("剑", 2, 4321);

        }

        //读取Script_Data的数据 获取某个武器的升级价格
        public int LoadScriptTable_Data_Weapon_Price(string weaponName, int level)
        {
            for (int i = 0; i < gameData_test.weaponsData.Length; i++)
            {
                if (gameData_test.weaponsData[i].name.Equals(weaponName))
                {
                    return gameData_test.weaponsData[i].Level_Pricel[level];
                }
            }
            Debug.Log("未能够找到数据！");
            return 0;
        }

        //存储数据  
        public bool SaveScriptTable_Data_Weapon_Price(string weaponName, int level, int price)
        {
            for (int i = 0; i < gameData_test.weaponsData.Length; i++)
            {
                if (gameData_test.weaponsData[i].name.Equals(weaponName))
                {
                    gameData_test.weaponsData[i].Level_Pricel[level] = price;
                    return true;
                }
            }
            Debug.Log("未能够找到数据！");
            return false;
        }
        ////---------------------------拓展↓↓↓-------------------------------------------------
    }
}
