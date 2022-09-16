/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    游戏管理器(主入口)
 * 公开方法：   
 * ChangeState(GameState gameState)             改变状态
 * ShowPanel() 动态加载某块UI      
 * ClosePanelofTop() 关闭隐藏顶层UI    
 * CloseAllPanel() 关闭所有UI
 * ShowJoystick()  显示玩家摇杆()
 * CloseJoystick() 隐藏玩家摇杆()
 * GetScenceIndex() 获取当前地图的编号
 * GameManager处理多个逻辑层的交互问题, 单个逻辑层只处理本逻辑层的交互，之后在EventManager注册事件.
 * 通过事件进行调用
**/

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
class GameManager : Sigleton<GameManager>
{
    //UI控制器(此处其实可以把UI分离单独做一个单例，但为了好管理，一并交给主脚本管理，主脚本引用子脚本的方法，内容归子脚本去实现)
    UIManager uiManager; 

    //世界控制器
    GameWorldManager gameWorldManager;

    //数据管理器
    DataManager dataManager;

    //虚拟摄像机控制器
    CinemaChineManager cinemaChineManager;

    //对象池管理器
    PoolManager poolManger;

    //游戏摇杆
    JoystickManager joystickManager;

    //游戏状态机
    StateMachine<GameManager> fsm_stateMachine;

    //游戏开始状态
    private GameBeginState gameBeginState;

    //游戏运行状态
    private GameRunState gameRunState;

    //游戏结束状态
    private GameEndState gameEndState;

    //游戏的全局状态
    private GameGlobalState gameGlobalState;

    [SerializeField,ReadOnly]
    //当前状态显示
    GameState curState;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //初始化管理器配置
        Init_Config();
        //限制60帧
        Application.targetFrameRate = dataManager.targetFrameRate;
        //初始化游戏内容
        Init_Game(); 
    }

    void Update()
    {
        if (fsm_stateMachine != null)
        {
            fsm_stateMachine.OnUpdate();
        }        
    }

    #region 整个游戏的初始化入口
    //初始化GameManager配置
    private void Init_Config()
    {
        uiManager = UIManager.Instance;
        gameWorldManager = GameWorldManager.Instance;
        dataManager = DataManager.Instance;
        cinemaChineManager = CinemaChineManager.Instance;
        poolManger = PoolManager.Instance;
        joystickManager = JoystickManager.Instance;

        //初始化游戏主状态机(Main)
        fsm_stateMachine = new StateMachine<GameManager>(this);
        //初始化各种状态(Main)
        InitState();
    }

    private void Init_Game()
    { 
        //初始化数据
        dataManager.Init();
        //初始化UI管理器
        uiManager.Init();
        //初始化对象池
        Init_Pool();
        //初始化摄像机
        Init_Camera();
        //初始化游戏世界
        Init_GameWorld();
        //设置初始游戏状态(Main)
        SetCurrentState(GameState.GameBegin);
        //设置全局状态 - 如果有
        //fsm_stateMachine.SetGlobalState((int)GameState.GameGlobal);
        //第一次打点 --> 游戏开始
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World01", "Level" + GetCurrentLevel_String());
    }

    //初始化各种状态
    private void InitState()
    {
        gameBeginState = new GameBeginState();
        gameRunState = new GameRunState();
        gameEndState = new GameEndState();
        gameGlobalState = new GameGlobalState();

        fsm_stateMachine.AddState((int)GameState.GameBegin, gameBeginState);
        fsm_stateMachine.AddState((int)GameState.GameRun, gameRunState);
        fsm_stateMachine.AddState((int)GameState.GameEnd, gameEndState);
        fsm_stateMachine.AddState((int)GameState.GameGlobal, gameGlobalState);
    }

    //改变当前游戏状态
    public void ChangeState(GameState gameState)
    {
        curState = gameState;
        fsm_stateMachine.ChangeState((int)gameState);
    }

    //初始化游戏当前状态
    private void SetCurrentState(GameState gameState)
    {
        curState = gameState;
        fsm_stateMachine.SetCurrentState((int)gameState);   
    }

    //游戏加载(实例化一些地图，场景角色等，小游戏不用考虑过渡问题，直接删除后重建)
    public void Init_GameWorld()
    {
        //初始化世界管理器(GameWorld) 参数：需要加载的关卡 
        gameWorldManager.Init();
        //可以在此处增加一些其他设定
        //--(Content)--
    }

    #endregion

    #region 游戏结算
    //游戏胜利
    public void GameWin()
    {
        //--这里写游戏结束时执行的内容
        //--(Content)--
        //改变游戏的状态 -> 游戏结束
        ChangeState(GameState.GameEnd);
    }

    //游戏失败
    public void GameLose()
    {
        //--这里写游戏结束时执行的内容
        //--(Content)--
        //改变游戏状态 -> 游戏结束
        ChangeState(GameState.GameEnd);
    }
   

    #endregion


    //重置所有数据信息
    public void RestAllData()
    {

    }

    //对象池初始化
    public void Init_Pool()
    {
        poolManger.Init();
        //TODO：可以在此处进行对象池的创建
        //--(Content)--
    }

    //初始化相机
    public void Init_Camera()
    {
        //更改相机为0号相机
        cinemaChineManager.ChangeCamera(0);
        //设置摄影机跟随的目标
        //SetTarget(GetPlayer());
        //设置相机观察目标
        //SetLookTarget(GetPlayer());
    }
}