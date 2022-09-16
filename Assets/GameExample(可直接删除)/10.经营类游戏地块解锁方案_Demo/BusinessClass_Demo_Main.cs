/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyDemo
{
    public class BusinessClass_Demo_Main : Sigleton<BusinessClass_Demo_Main>
    {

        //游戏数据
        public GameDataManager_Demo m_gameData;

        //摄影机
        public Camera_test Camera_Test;

        //玩家实体
        public Player_test Player_test;

        //存储地块信息
        public Plane_Demo[] planeInfo_Datas;

        //把未解锁的地块信息添加进列表
        public List<Plane_Demo> planeInfo_Lock;

        //------------ 因为Demo涉及到的UI元素不多，所以直接丢Main上了
        //金币面板
        public Text goldText;

        //临时金币面板(做金币跳动的动画用)
        public Text goldText_temp;

        //声明动画库
        private UIAnimation uiAnimation = new UIAnimation();

        //------------------------------------

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            Init();

        }

        private void Update()
        {
            //点击一下空格增加金币
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddGold();

                FlashingPlane();
            }
        }

        //初始化
        private void Init()
        {
            //初始化数据
            m_gameData.Init();
            //初始化相机
            Camera_Test.Init(Player_test.transform);
            //初始化地块的解锁情况
            InitPlane();
        }

        //初始化地块的解锁信息(明亮代表解锁，黑色代表未解锁) 根据所读取的信息来设置
        private void InitPlane()
        {
            for (int i = 0; i < planeInfo_Datas.Length; i++)
            {
                if (m_gameData.IsUnLockLevel(i))
                {
                    SetBlockBrightness_Light(i);
                }
                else
                {
                    SetBlockBrightness_Dark(i);
                    //未解锁的地块添加进列表
                    planeInfo_Lock.Add(planeInfo_Datas[i]);
                }
            }
        }

        //使下一个等级的地块进行闪烁显示
        private void FlashingPlane()
        {
            int Player_Level = m_gameData.GetPlayer_Level();
            //检测玩家的权限等级是否达到关卡等级的最大值，同时检测玩家的金币数量，
            //如果条件都符合，则让下一等级的地块闪烁
            if (Player_Level < m_gameData.m_PlaneData.PlaneNum)
            {
                if (m_gameData.GetPlayer_Gold() >= m_gameData.GetUnLockGold(Player_Level))
                {
                    //遍历未解锁地块中的等级,符合要求则开启闪烁
                    for (int i = 0; i < planeInfo_Lock.Count; i++)
                    {
                        if (!planeInfo_Lock[i].isOpenFlash && planeInfo_Lock[i].Plane_Level == m_gameData.GetPlayer_Level() + 1)
                        {
                            planeInfo_Lock[i].isOpenFlash = true;
                            //显示解锁触发区域
                            planeInfo_Lock[i].ShowUnLockTriggerArea(true);
                        }
                    }
                }
            }

        }

        //设置地块为明亮(可同行)
        private void SetBlockBrightness_Light(int num)
        {
            planeInfo_Datas[num].gameObject.GetComponent<MeshRenderer>().material.SetFloat("_RampThreshold", 0.7f);
        }

        //设置地块为灰色(不可通行)
        private void SetBlockBrightness_Dark(int num)
        {
            planeInfo_Datas[num].gameObject.GetComponent<MeshRenderer>().material.SetFloat("_RampThreshold", 1.0f);
        }


        //增加玩家金币数量(显示层 + 数据层)
        public void AddGold()
        {
            uiAnimation.JumpNum_Child(goldText_temp, m_gameData.GetPlayer_Gold(), m_gameData.GetPlayer_Gold() + 20, 2, () =>
            {
                m_gameData.Add_Player_Gold(20);
                goldText.text = "Gold :" + m_gameData.GetPlayer_Gold();
                goldText_temp.gameObject.SetActive(false);
            });
        }

        //减少玩家的金币数量(显示层 + 数据层)
        public void RemoveGold(int num)
        {
            uiAnimation.JumpNum_Child(goldText_temp, m_gameData.GetPlayer_Gold(), m_gameData.GetPlayer_Gold() - num, 2, () =>
            {
                m_gameData.Remove_Player_Gold(num);
                goldText.text = "Gold :" + m_gameData.GetPlayer_Gold();
                goldText_temp.gameObject.SetActive(false);
            });
        }

        //解锁地块 (显示层 + 数据层)
        public void UnLockPlane(Plane_Demo plane_Demo)
        {
            plane_Demo.UnLockPlane();
            m_gameData.UnLockLevel(plane_Demo.Plane_Level);
        }
    }

}

