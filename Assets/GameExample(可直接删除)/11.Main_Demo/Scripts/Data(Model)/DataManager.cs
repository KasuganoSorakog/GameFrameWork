/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    数据少可直接放此处，数据多时需要进行分类
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class DataManager : MonoBehaviour
    {
        //玩家信息(如果没有可删除)
        //PlayerInfor m_PlayerInfor;

        [Header("玩家的行走速度"), Range(1f, 10f)]
        public float PlyaerSpeed_walk = 10;

        [Header("玩家的转弯系数"), Range(1, 20f)]
        public float turnSpeed = 8;

        [Header("游戏进入结算的延迟时间"), Range(1.0f, 10.0f)]
        public float GameEnd_delayTime = 1;

        //玩家每一关的出生位置可能有所不同
        [Header("玩家的初始位置")]
        public Transform[] PlayerPos;

        //当前游戏中已有关卡数(在游戏开始时初始化)
        [HideInInspector]
        public int LevelNum;

        [HideInInspector]
        public int LevelScore;

        public void Awake()
        {
            Init();
        }

        //初始化
        private void Init()
        {

        }

        //获取每一关玩家的出生位置(根据所加载的关卡进行设定)
        public Vector3 GetPlayerInitPos()
        {
            //根据当前所加载的关卡的Index，获取玩家位置
            int num = GameManager.Instance.GetScenceIndex();

            if (num <= PlayerPos.Length)
            {
                return PlayerPos[num - 1].position;
            }
            else
            {
                Debug.Log("您当前的关卡数和所设位置玩家点位的数量不符！！");
                return Vector3.zero;
            }
        }

        //重置分数
        public void RestScore()
        {
            LevelScore = 0;
        }
    }

}
