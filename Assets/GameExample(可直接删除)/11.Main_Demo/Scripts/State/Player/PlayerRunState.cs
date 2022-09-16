/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-15 
 *说明:    玩家的跑步状态, 设定玩家的跑步逻辑
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    class PlayerRunState : State<Player>
    {
        public override void Enter(Player player)
        {
            player.PlayAnimation("Run");
        }

        public override void Excute(Player player)
        {
            if (player.CanMove)
            {

            }
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
