/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-24 
 *说明:    用于管理多个系统之间的信息交互
**/  

using UnityEngine;  
using System.Collections;
using DG.Tweening;
using Cinemachine;

namespace MyDemo
{
    public class EventManager : MonoBehaviour
    {
        private void Awake()
        {
            AddListener();
        }

        private void OnDestroy()
        {
            RemoveLsitenr();
        }

        #region UI的相关方法
        //显示某个UI窗口
        private void ShowPanel(string PanelType)
        {
            GameManager.Instance.ShowPanel(PanelType);
        }

        //关闭UI最顶层窗口
        private void ClosePanelofTop()
        {
            GameManager.Instance.ClosePanelofTop();
        }

        //关闭UI所有窗口
        private void CloseAllPanel()
        {
            GameManager.Instance.CloseAllPanel();
        }

        //进入下一关(按钮事件)
        private void OnNextOnClick()
        {
            GameSaveManager.Save<int>(DataType.Level, GameSaveManager.Load<int>(DataType.Level) + 1);
            GameManager.Instance.RestAllData();
            GameManager.Instance.LoadGame();
            GameManager.Instance.ChangeState(GameState.GameBegin);
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World01", "Level" + GameManager.Instance().GetCurrentLevel_String());
            //Debug.Log("开始事件：进入了" + GameManager.Instance().GetCurrentLevel_String() + "关");

        }

        //重新开始游戏(按钮事件)
        private void OnReplayBtnOnClick()
        {
            GameManager.Instance.RestAllData();
            GameManager.Instance.LoadGame();
            GameManager.Instance.ChangeState(GameState.GameBegin);
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World01", "Level" + GameManager.Instance().GetCurrentLevel_String());
            //Debug.Log("开始事件：进入了" + GameManager.Instance().GetCurrentLevel_String() + "关");
        }

        //增加分数
        private void AddScore()
        {
            GameManager.Instance.AddScore();
        }

        //减少分数
        private void RemoveScore()
        {
            GameManager.Instance.RemoveScore();
        }

        #endregion

        #region 玩家的相关事件
        //发送信息给玩家
        private void HandleMessage_Player(MessageType playerMessage, params object[] parm)
        {
            GameManager.Instance.HandleMessage_Player(playerMessage, parm);
        }

        #endregion

        #region 相机的相关事件
        //设置相机跟随目标 
        public void SetTarget(Transform obj)
        {
            GameManager.Instance.SetTarget(obj);
        }

        //设置相机注释目标
        public void SetLookTarget(Transform obj)
        {
            GameManager.Instance.SetLookTarget(obj);
        }

        //切换虚拟相机
        public void ChangeCamera(int index)
        {
            GameManager.Instance.ChangeCamera(index);
        }

        //改变相机的刷新方式
        public void SetUpdateMethod(CinemachineBrain.UpdateMethod method)
        {
            GameManager.Instance.SetUpdateMethod(method);
        }


        #endregion

        #region 游戏结算
        //游戏胜利
        private void GameWin()
        {
            GameManager.Instance.GameWin();
        }

        //游戏失败
        private void GameLose()
        {
            GameManager.Instance.GameLose();
        }
        #endregion

        //添加事件监听
        private void AddListener()
        {
            //UI窗口的操作
            Messenger.AddListener<string>(EventType.ShowPanel, ShowPanel);
            Messenger.AddListener(EventType.ClosePanelofTop, ClosePanelofTop);
            Messenger.AddListener(EventType.CloseAllPanel, CloseAllPanel);

            Messenger.AddListener(EventType.OnNextOnClick, OnNextOnClick);
            Messenger.AddListener(EventType.OnReplayBtnOnClick, OnReplayBtnOnClick);

            Messenger.AddListener(EventType.AddScore, AddScore);
            Messenger.AddListener(EventType.RemoveScore, RemoveScore);

            //玩家操作
            Messenger.AddListener<MessageType, object[]>(EventType.HandleMessage_Player, HandleMessage_Player);

            //相机的操作
            Messenger.AddListener<Transform>(EventType.SetTarget, SetTarget);
            Messenger.AddListener<Transform>(EventType.SetLookTarget, SetLookTarget);
            Messenger.AddListener<CinemachineBrain.UpdateMethod>(EventType.SetUpdateMethod, SetUpdateMethod);
            Messenger.AddListener<int>(EventType.ChangeCamera, ChangeCamera);

            //游戏结算
            Messenger.AddListener(EventType.GameWin, GameWin);
            Messenger.AddListener(EventType.GameLose, GameLose);

        }

        //移除事件监听
        private void RemoveLsitenr()
        {
            //UI窗口的操作
            Messenger.RemoveListener<string>(EventType.ShowPanel, ShowPanel);
            Messenger.RemoveListener(EventType.ClosePanelofTop, ClosePanelofTop);
            Messenger.RemoveListener(EventType.CloseAllPanel, CloseAllPanel);

            Messenger.RemoveListener(EventType.OnNextOnClick, OnNextOnClick);
            Messenger.RemoveListener(EventType.OnReplayBtnOnClick, OnReplayBtnOnClick);

            Messenger.RemoveListener(EventType.AddScore, AddScore);
            Messenger.RemoveListener(EventType.RemoveScore, RemoveScore);

            //玩家操作
            Messenger.RemoveListener<MessageType, object[]>(EventType.HandleMessage_Player, HandleMessage_Player);

            //相机的操作
            Messenger.RemoveListener<Transform>(EventType.SetTarget, SetTarget);
            Messenger.RemoveListener<Transform>(EventType.SetLookTarget, SetLookTarget);
            Messenger.RemoveListener<CinemachineBrain.UpdateMethod>(EventType.SetUpdateMethod, SetUpdateMethod);
            Messenger.RemoveListener<int>(EventType.ChangeCamera, ChangeCamera);

            //游戏结算
            Messenger.RemoveListener(EventType.GameWin, GameWin);
            Messenger.RemoveListener(EventType.GameLose, GameLose);
        }
    }
}
