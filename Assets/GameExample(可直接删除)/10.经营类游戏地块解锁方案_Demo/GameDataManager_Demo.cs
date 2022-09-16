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
using UnityEditor;
using System;
using MyDemo;

namespace MyDemo
{
    public class GameDataManager_Demo : MonoBehaviour
    {

        //玩家的游戏数据
        public GameDataSave_Demo m_gameDataSave;

        //游戏的参数设定
        public Plane_Data m_PlaneData;

        public void Init()
        {
            //尝试获取是否有文件
            m_gameDataSave = GameDataSaveManager_Demo.LoadData();

            //当读取到的文件为空的时候，则新建一份存储
            m_gameDataSave.IsNull().Do(() =>
            {
                List<bool> IsUnLockLevel = new List<bool>();
                for (int i = 0; i < m_PlaneData.PlaneNum; i++)
                {
                    i.IsZero().Do(() => IsUnLockLevel.Add(true));

                    i.IsNotZero().Do(() => IsUnLockLevel.Add(false));
                }
                m_gameDataSave = new GameDataSave_Demo() { IsUnLockLevel = IsUnLockLevel, player_Level = 1 };
                GameDataSaveManager_Demo.SaveData(m_gameDataSave);
            });
        }

        //将某一个关卡的数据解锁
        public void UnLockLevel(int num)
        {
            if (num < m_gameDataSave.IsUnLockLevel.Count)
            {
                m_gameDataSave.IsUnLockLevel[num] = true;
            }
        }

        //读取某一个关卡的数据(是否解锁)
        public bool IsUnLockLevel(int num)
        {
            return m_gameDataSave.IsUnLockLevel[num];
        }

        //读取某个等级地块解锁所需金币
        public int GetUnLockGold(int num)
        {
            return m_PlaneData.gold[num];
        }

        //读取玩家金币
        public int GetPlayer_Gold()
        {
            return m_gameDataSave.player_Gold;
        }

        //存储玩家金币
        public int Add_Player_Gold(int num)
        {
            m_gameDataSave.player_Gold += num;
            return m_gameDataSave.player_Gold;
        }

        //减少玩家金币
        public int Remove_Player_Gold(int num)
        {
            m_gameDataSave.player_Gold -= num;
            return m_gameDataSave.player_Gold;
        }

        //读取玩家等级
        public int GetPlayer_Level()
        {
            return m_gameDataSave.player_Level;
        }

        //存储玩家的等级
        public int Add_Player_Leve()
        {
            m_gameDataSave.player_Level += 1;
            return m_gameDataSave.player_Level;
        }


    }

}
