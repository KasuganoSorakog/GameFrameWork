/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    玩家所挂的脚本，负责控制玩家的行为
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //人物的状态机
    StateMachine<Player> fsm_stateMachine = null;

    //人物的Idle状态
    private PlayerIdleState playerIdleState;

    //人物的走路状态
    private PlayerWalkState playerWalkState;

    //人物的跑步状态
    private PlayerRunState playerRunState;

    //人物的胜利状态
    private PlayerWinState playerWinState;

    //人物死亡状态
    private PlayerDieState playerDieState;

    //人物的全局状态
    private PlayerGlobalState playerGlobalState;

    //当前状态显示
    [SerializeField, ReadOnly]
    PlayerState curState;

    //角色初始化
    private void Init()
    {
        fsm_stateMachine = new StateMachine<Player>(this);
        InitState();
        //设置当前状态
        SetCurrentState(PlayerState.Idle);
        //设置全局状态 - 如果有
        //fsm_stateMachine.SetGlobalState((int)PlayerState.PlayerGlobal);
    }

    //初始化各种状态
    private void InitState()
    {
        playerIdleState = new PlayerIdleState();
        playerRunState = new PlayerRunState();
        playerWalkState = new PlayerWalkState();
        playerWinState = new PlayerWinState();
        playerDieState = new PlayerDieState();
        playerGlobalState = new PlayerGlobalState();

        fsm_stateMachine.AddState((int)PlayerState.Idle, playerIdleState);
        fsm_stateMachine.AddState((int)PlayerState.Walk, playerRunState);
        fsm_stateMachine.AddState((int)PlayerState.Run, playerWalkState);
        fsm_stateMachine.AddState((int)PlayerState.Win, playerWinState);
        fsm_stateMachine.AddState((int)PlayerState.Die, playerDieState);
        fsm_stateMachine.AddState((int)PlayerState.PlayerGlobal, playerGlobalState);
    }

    //改变当前游戏状态
    public void ChangeState(PlayerState playerState)
    {
        curState = playerState;
        fsm_stateMachine.ChangeState((int)playerState);        
    }

    //初始化游戏当前状态
    private void SetCurrentState(PlayerState playerState)
    {
        curState = playerState;
        fsm_stateMachine.SetCurrentState((int)playerState);
    }
}


