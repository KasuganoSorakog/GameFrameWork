/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    游戏的结束状态
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    class GameEndState : State<GameManager>
    {
        GameTimer delTime = new GameTimer();

        public override void Enter(GameManager gameManager)
        {
            //设置延迟时间(默认为1秒)
            delTime.Set(gameManager.m_DataManger.GameEnd_delayTime);
            //关闭"摇杆窗口"
            gameManager.CloseJoystick();
            //关闭BasicPanel窗口
            gameManager.ClosePanelofTop();
        }

        public override void Excute(GameManager gameManager)
        {
            //失败后，延迟结束
            if (delTime.GetEnable() && delTime.IsTimeOut())
            {
                //根据游戏结果显示不同的窗口
                if (gameManager.GameResult)
                {
                    //显示"胜利窗口"
                    gameManager.ShowPanel(UIPanelType.WinPanel);
                }
                else
                {
                    //显示"失败窗口"
                    gameManager.ShowPanel(UIPanelType.FailPanel);
                }
                delTime.SetEnable(false);
            }
        }

        public override void Exit(GameManager gameManager)
        {
            //关闭最上层窗口
            gameManager.ClosePanelofTop();
        }
    }
}
