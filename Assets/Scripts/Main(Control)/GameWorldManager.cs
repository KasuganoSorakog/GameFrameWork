/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    游戏世界管理器， 应该管理世界场景中的各种游戏物体，比如玩家，比如怪物
 *         游戏主管理器(GameMain)可以从中访问到个体, 并且传递消息
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class GameWorldManager : Sigleton<GameWorldManager>
{

    public Player player;

    //加载场景中的物体(初始化的操作应该放于GameMain)
    public void Init()
    {
        
    }


    #region 场景的初始化方法内容

    #endregion

    #region 玩家的操作方法

    #endregion
}
