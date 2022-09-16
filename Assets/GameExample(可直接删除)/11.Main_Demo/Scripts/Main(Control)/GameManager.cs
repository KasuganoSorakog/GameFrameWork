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
**/

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo 
{
    class GameManager : Sigleton<GameManager>
    {
        //UI控制器(此处其实可以把UI分离单独做一个单例，但为了好管理，一并交给主脚本管理，主脚本引用子脚本的方法，内容归子脚本去实现)
        public UIManager m_UIManager = null;

        //世界控制器
        public GameWorldManager m_gameWorldManager = null;

        //虚拟摄像机控制器
        public CinemaChineManager m_CinemaChineManager = null;

        //数据管理器
        public DataManager m_DataManger = null;

        //对象池管理器
        public PoolManager m_PoolManger = null;

        //游戏状态机
        StateMachine<GameManager> FSM_stateMachine = null;

        //游戏开始状态
        private GameBeginState gameBeginState;

        //游戏运行状态
        private GameRunState gameRunState;

        //游戏结束状态
        private GameEndState gameEndState;

        //游戏运行状态的字典
        Dictionary<GameState, State<GameManager>> gameStateDic;

        //游戏的结果 (胜利/失败)
        [HideInInspector]
        public bool GameResult;

        protected override void Awake()
        {
            base.Awake();
            //初始化GA，FackBook和Adjust打点
            Init_FB_GA_Adjust();
            //初始化游戏状态字典(Main)
            gameStateDic = new Dictionary<GameState, State<GameManager>>();
            //初始化游戏主状态机(Main)
            FSM_stateMachine = new StateMachine<GameManager>(this);
        }

        void Start()
        {
            //初始化游戏所有的内容
            Init();        
        }

        void Update()
        {
            if (FSM_stateMachine != null)
            {
                FSM_stateMachine.Update();
            }
        }

        #region 整个游戏的初始化入口
        private void Init()
        {
            ////初始化各种状态(Main)
            InitState();
            //初始化显示层信息
            InitUIData();
            //初始化游戏中所存有的关卡数
            //InitLevelNum();
            //加载游戏
            LoadGame();
            ////设置初始游戏状态(Main)
            SetCurrentState(GameState.GameBegin);
            //第一次打点 --> 游戏开始
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World01", "Level" + GetCurrentLevel_String());
        }

        //初始化各种状态
        private void InitState()
        {
            gameBeginState = new GameBeginState();
            gameRunState = new GameRunState();
            gameEndState = new GameEndState();

            gameStateDic.Add(GameState.GameBegin, gameBeginState);
            gameStateDic.Add(GameState.GameRun, gameRunState);
            gameStateDic.Add(GameState.GameEnd, gameEndState);
        }

        //初始化游戏中的关卡数
        private void InitLevelNum()
        {
            //根据Resource文件夹里面的资源数进行加载
            Object[] a = Resources.LoadAll("Level", typeof(GameObject));
            m_DataManger.LevelNum = a.Length;
        }

        //改变当前游戏状态
        public void ChangeState(GameState gameState)
        {
            if (gameStateDic.GetValue(gameState) != null)
            {
                FSM_stateMachine.ChangeState(gameStateDic.GetValue(gameState));
            }
        }

        //初始化游戏当前状态
        private void SetCurrentState(GameState playerState)
        {
            if (gameStateDic.GetValue(playerState) != null)
            {
                FSM_stateMachine.SetCurrentState(gameStateDic.GetValue(playerState));
            }
        }

        //游戏加载(实例化一些地图，场景角色等，小游戏不用考虑过渡问题，直接删除后重建)
        public void LoadGame()
        {
            //初始化世界管理器(GameWorld) 参数：需要加载的关卡 
            m_gameWorldManager.GameWorld_Init(GameSaveManager.Load<int>(DataType.Level));
        }

        //初始化GA，FaceBook和Adjust打点
        private void Init_FB_GA_Adjust()
        {
            //GA的初始化
            //GameAnalytics.Initialize();
            ////FB的初始化
            //if (!FB.IsInitialized)
            //{
            //    FB.Init();
            //}
            //else
            //{
            //    FB.ActivateApp();
            //}
            ////初始化Adjust(TODO: 此处要根据实际项目更改)
            //InitAdjust("");
        }

        //初始化Adjust
        private void InitAdjust(string adjustAppToken)
        {
            //var adjustConfig = new AdjustConfig(
            //    adjustAppToken,
            //    AdjustEnvironment.Production, // AdjustEnvironment.Sandbox to test in dashboard
            //    true
            //);
            //adjustConfig.setLogLevel(AdjustLogLevel.Info); // AdjustLogLevel.Suppress to disable logs
            //adjustConfig.setSendInBackground(true);
            //new GameObject("Adjust").AddComponent<Adjust>(); // do not remove or rename
            //Adjust.start(adjustConfig);


            //------ 此处的注释可以不用取消
            // Adjust.addSessionCallbackParameter("foo", "bar"); // if requested to set session-level parameters

            //adjustConfig.setAttributionChangedDelegate((adjustAttribution) => {
            //  Debug.LogFormat("Adjust Attribution Callback: ", adjustAttribution.trackerName);
            //});
        }

        #endregion

        #region 数据获取
        //获取当前关卡场景的Index，查看正在加载哪一关(真实关卡)
        public int GetScenceIndex()
        {
            return m_gameWorldManager.GetScenceIndex();
        }

        //获取玩家的初始位置
        public Vector3 GetPlayerInitPos()
        {
            return m_DataManger.GetPlayerInitPos();
        }

        //返回当前正在进行的关卡数的string
        public string GetCurrentLevel_String()
        {
            int Level = GameSaveManager.Load<int>(DataType.Level);
            string temp;
            if (Level < 10)
            {
                temp = "00" + Level.ToString();
            }
            else if (Level >= 10 && Level < 100)
            {
                temp = "0" + Level.ToString();
            }
            else
            {
                temp = Level.ToString();
            }
            return temp;
        }

        #endregion

        #region 游戏结算
        //游戏胜利
        public void GameWin()
        {
            //设置胜负结果
            GameResult = true;
            //可以给人物添加胜利特效(TODO：此处可以添加胜利的事件，比如人物做个动作啊，播放个特效)
            //--------------------------
            //禁止玩家移动(除此之外，可能还要调整一些东西)
            SetPlayerMove(false);
            //改变游戏的状态 -> 游戏结束
            ChangeState(GameState.GameEnd);
        }

        //游戏失败
        public void GameLose()
        {
            //设置胜负结果
            GameResult = false;
            //可以给人物添加胜利特效(TODO：此处可以添加胜利的事件，比如人物做个动作啊，播放个特效)
            //-------------------------
            //禁止玩家移动(除此之外，可能还要调整一些东西)
            SetPlayerMove(false);
            //改变游戏状态 -> 游戏结束
            ChangeState(GameState.GameEnd);
        }

        //游戏启动时候的方法集合
        public void GameRun()
        {
            //设置玩家可以移动
            SetPlayerMove(true);
        }

        //判断游戏是否结束
        public bool IsGameEnd()
        {
            if (FSM_stateMachine.CurrentState() == gameEndState)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region UI相关方法 & 数据处理
        //开启UI窗口
        public void ShowPanel(string PanelType, UIPanelData uiPanelData = null)
        {
            m_UIManager.ShowPanel(PanelType, uiPanelData);
        }

        //关闭UI最顶层窗口
        public void ClosePanelofTop()
        {
            m_UIManager.ClosePanelOfTop();
        }

        //关闭UI所有窗口
        public void CloseAllPanel()
        {
            m_UIManager.CloseAllPanel();
        }

        //显示虚拟摇杆
        public void ShowJoystick()
        {
            m_UIManager.ShowJoystick();

        }

        //隐藏虚拟摇杆
        public void CloseJoystick()
        {
            m_UIManager.CloseJoystick();
        }

        //增加分数
        public void AddScore()
        {
            //更新到数据层
            m_DataManger.LevelScore++;
            //更新到显示层
            BasicPanel panel = m_UIManager.GetPanel(UIPanelType.BasicPanel) as BasicPanel;
            panel.Update_ScoreNum(m_DataManger.LevelScore);
        }

        //减少分数
        public void RemoveScore()
        {
            //更新到数据层
            m_DataManger.LevelScore--;
            //更新到显示层
            BasicPanel panel = m_UIManager.GetPanel(UIPanelType.BasicPanel) as BasicPanel;
            panel.Update_ScoreNum(m_DataManger.LevelScore);
        }

        //初始化所有显示层信息
        public void InitUIData()
        {
            BasicPanel panel = m_UIManager.GetPanel(UIPanelType.BasicPanel) as BasicPanel;
            panel.InitUIData();

        }

        //重置所有数据信息
        public void RestAllData()
        {
            m_DataManger.RestScore();
            BasicPanel panel = m_UIManager.GetPanel(UIPanelType.BasicPanel) as BasicPanel;
            panel.ResetAllData();
        }

        #endregion

        #region 对象池相关方法(使用之前需要在PoolManager中实例化出对应的对象池)
        //获取一个对象
        public void GetObject(PoolName poolName)
        {
            m_PoolManger.GetObject(poolName);
        }

        //释放对象
        public void ReleaseObject(GameObject obj)
        {
            m_PoolManger.ReleaseObject(obj);
        }
        #endregion

        #region 世界管理器相关方法
        //设置玩家是否能够移动
        public void SetPlayerMove(bool canMove)
        {
            m_gameWorldManager.SetPlayerMove(canMove);
        }

        //设置玩家的移动速率(此方法不单独作为消息使用，一般是配合某种状态，比如吃了某个加速道具)
        public void SetSpeedMul_Player(float num)
        {
            m_gameWorldManager.SetSpeedMul_Player(num);
        }

        //获取玩家角色
        public Transform GetPlayer()
        {
            return m_gameWorldManager.GetPlayer();
        }

        //改变角色状态
        public void ChangePlayerState(PlayerState state)
        {
            m_gameWorldManager.ChangePlayerState(state);
        }

        //通知玩家播放某个动作
        public void PlayAnimation_Player(string triggleName)
        {
            m_gameWorldManager.PlayAnimation_Player(triggleName);
        }

        public void HandleMessage_Player(MessageType playerMessage, params object[] parm)
        {
            m_gameWorldManager.HandleMessage_Player(playerMessage, parm);
        }

        #endregion

        #region 摄像机的相关方法

        //初始化相机
        public void InitCamera()
        {
            //更改相机为0号相机
            ChangeCamera(0);
            //设置摄影机跟随的目标
            SetTarget(GetPlayer());
            //设置相机观察目标
            SetLookTarget(GetPlayer());
        }

        //设置相机跟随目标 
        public void SetTarget(Transform obj)
        {
            m_CinemaChineManager.SetTarget(obj);
        }

        //设置相机注释目标
        public void SetLookTarget(Transform obj)
        {
            m_CinemaChineManager.SetLookTarget(obj);
        }

        //更换虚拟相机
        public void ChangeCamera(int index)
        {
            m_CinemaChineManager.ChangeCamera(index);
        }

        //改变相机的刷新方式
        public void SetUpdateMethod(CinemachineBrain.UpdateMethod method)
        {
            m_CinemaChineManager.SetUpdateMethod(method);
        }

        #endregion

    }



}
