/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-24 
 *说明:    添加新状态时，需要更新此处的枚举，以及状态主体本身的字典
**/  

//游戏运行状态
public enum GameState
{
    GameBegin,
    GameRun,
    GameEnd,
    GameGlobal          //全局状态
}

//人物的状态
public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Win,
    Die,
    PlayerGlobal        //全局状态
}
