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

class GameBeginState : State<GameManager> 
{
    public override void Enter(GameManager gameManager)
    {
        JoystickManager.Instance.ShowJoystick(false);
    }

    public override void Excute(GameManager gameManager)
    {

    }

    public override void Exit(GameManager gameManager)
    {

    }

}  