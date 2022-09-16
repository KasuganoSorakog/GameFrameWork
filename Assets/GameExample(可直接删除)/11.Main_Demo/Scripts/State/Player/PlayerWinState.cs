/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-08-05 
 *说明:    玩家胜利状态
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    class PlayerWinState : State<Player>
    {
        public override void Enter(Player player)
        {
            player.PlayAnimation("Win");
        }

        public override void Excute(Player player)
        {

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
