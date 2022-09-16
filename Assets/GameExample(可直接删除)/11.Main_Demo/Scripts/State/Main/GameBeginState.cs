/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    游戏的开始状态(游戏最开始的入口)
 * 
**/

using UnityEngine;
using System.Collections;

namespace MyDemo
{
    class GameBeginState : State<GameManager>
    {
        public override void Enter(GameManager gameManager)
        {
            //显示"基础按钮"
            gameManager.ShowPanel(UIPanelType.BasicPanel);
            //显示"开始窗口"
            gameManager.ShowPanel(UIPanelType.StartPanel);
            //显示"摇杆窗口"
            gameManager.ShowJoystick();
            //初始化相机
            gameManager.InitCamera();
        }

        public override void Excute(GameManager gameManager)
        {
            if (Input.anyKeyDown)
            {
                //关闭"开始窗口"
                GameManager.Instance.ClosePanelofTop();
                //改变游戏状态
                gameManager.ChangeState(GameState.GameRun);
            }
        }

        public override void Exit(GameManager gameManager)
        {

        }
    }
}
