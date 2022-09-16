/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-24 
 *说明:    事件类型..该类只负责存储事件类型的string， 可以直接通过类名调用
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    public class EventType
    {
        //游戏主管理器的类型参数
        public const string ChangeGameMainState = "ChangeGameMainState";

        //相关的UI的类型参数
        public const string ShowPanel = "ShowPanel";
        public const string ClosePanelofTop = "ClosePanelofTop";
        public const string CloseAllPanel = "CloseAllPanel";

        public const string OnNextOnClick = "OnNextOnClick";
        public const string OnReplayBtnOnClick = "OnReplayBtnOnClick";

        public const string AddScore = "AddScore";
        public const string RemoveScore = "RemoveScore";

        //相机的消息参数
        public const string SetTarget = "SetTarget";
        public const string SetLookTarget = "SetLookTarget";
        public const string SetUpdateMethod = "SetUpdateMethod";
        public const string ChangeCamera = "ChangeCamera";

        //玩家的消息参数
        public const string HandleMessage_Player = "HandleMessage_Player";

        //游戏结算
        public const string GameWin = "GameWin";
        public const string GameLose = "GameLose";

    }


}
