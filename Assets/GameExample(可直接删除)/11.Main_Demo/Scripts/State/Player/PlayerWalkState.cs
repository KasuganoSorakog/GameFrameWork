/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-15 
 *说明:    玩家的走动状态，设定玩家的走动逻辑
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    class PlayerWalkState : State<Player>
    {

        public override void Enter(Player player)
        {
            player.PlayAnimation("Walk");
        }

        public override void Excute(Player player)
        {
            if (player.CanMove)
            {
                //判断玩家是否能够移动
                if (Mathf.Abs(player.m_joystick.Horizontal) > 0.1f || Mathf.Abs(player.m_joystick.Vertical) > 0.1f)
                {
                    //接收遥感的控制，当没有输入的时候，就转变成idle状态
                    player.PlayerMove();
                }
                else
                {
                    player.ChangeState(PlayerState.Idle);
                }
            }
            else
            {
                player.ChangeState(PlayerState.Idle);
            }
        }

        public override void Exit(Player player)
        {

        }

        public override bool OnMessage(Player player, MessageType messageType, params object[] objs)
        {
            switch (messageType)
            {
                case MessageType.StartPlayerMove:
                    player.CanMove = true;
                    return true;
                case MessageType.StopPlayerMove:
                    player.CanMove = true;
                    return true;
                case MessageType.PlayAnimation_Player:
                    player.PlayAnimation(objs[0].ToString());
                    return true;
                case MessageType.SetPlayerSpeedMul:
                    player.SetSpeedMul_Player((float)objs[0]);
                    return true;
                case MessageType.ChangePlayerState:
                    player.ChangeState((PlayerState)objs[0]);
                    return true;
                default:
                    break;
            }
            return false;
        }
    }
}
