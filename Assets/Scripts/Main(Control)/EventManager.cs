/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-24 
 *说明:    用于管理多个系统之间的信息交互
 * 所有事件，尽可能在此处进行注册
**/  

using UnityEngine;  
using System.Collections;
using DG.Tweening;
using Cinemachine;

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
        UIManager.Instance.ShowPanel(PanelType);
    }

    //关闭UI最顶层窗口
    private void ClosePanelOfTop()
    {
        UIManager.Instance.ClosePanelOfTop();
    }

    //关闭UI所有窗口
    private void CloseAllPanel()
    {
        UIManager.Instance.CloseAllPanel();
    }

    private void ShowJoystick(bool isShow)
    {
        JoystickManager.Instance.ShowJoystick(isShow);
    }

    //UI面板里的事件(例子)
    //private void PanelEvntyName(int num)
    //{
    //    PanelType panel =  (PanelType)UIManager.Instance.GetPanel(UIPanelType.MyPanel);
    //    panel.xxx(num);
    //}

    //进入下一关(按钮事件)
    private void OnNextOnClick()
    {
        GameSaveManager.Save<int>(DataType.Level, GameSaveManager.Load<int>(DataType.Level) + 1);
        GameManager.Instance.RestAllData();
        GameManager.Instance.Init_GameWorld();
        GameManager.Instance.ChangeState(GameState.GameBegin);
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World01", "Level" + GameManager.Instance().GetCurrentLevel_String());
        //Debug.Log("开始事件：进入了" + GameManager.Instance().GetCurrentLevel_String() + "关");

    }

    //重新开始游戏(按钮事件)
    private void OnReplayBtnOnClick()
    {
        GameManager.Instance.RestAllData();
        GameManager.Instance.Init_GameWorld();
        GameManager.Instance.ChangeState(GameState.GameBegin);
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World01", "Level" + GameManager.Instance().GetCurrentLevel_String());
        //Debug.Log("开始事件：进入了" + GameManager.Instance().GetCurrentLevel_String() + "关");
    }

    #endregion

    #region 玩家的相关事件

    #endregion

    #region 相机的相关事件
    //设置相机跟随目标 
    public void SetTarget(Transform obj)
    {
        CinemaChineManager.Instance.SetTarget(obj);
    }

    //设置相机注释目标
    public void SetLookTarget(Transform obj)
    {
        CinemaChineManager.Instance.SetLookTarget(obj);
    }

    //切换虚拟相机
    public void ChangeCamera(int index)
    {
        CinemaChineManager.Instance.ChangeCamera(index);
    }

    //改变相机的刷新方式
    public void SetUpdateMethod(CinemachineBrain.UpdateMethod method)
    {
        CinemaChineManager.Instance.SetUpdateMethod(method);
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
        Messenger.AddListener<string>(UIEventType.ShowPanel, ShowPanel);
        Messenger.AddListener(UIEventType.ClosePanelofTop, ClosePanelOfTop);
        Messenger.AddListener(UIEventType.CloseAllPanel, CloseAllPanel);

        Messenger.AddListener(UIEventType.OnNextOnClick, OnNextOnClick);
        Messenger.AddListener(UIEventType.OnReplayBtnOnClick, OnReplayBtnOnClick);


        Messenger.AddListener<bool>(NormalEventType.ShowJoystick, ShowJoystick);
        //相机的操作
        Messenger.AddListener<Transform>(NormalEventType.SetTarget, SetTarget);
        Messenger.AddListener<Transform>(NormalEventType.SetLookTarget, SetLookTarget);
        Messenger.AddListener<CinemachineBrain.UpdateMethod>(NormalEventType.SetUpdateMethod, SetUpdateMethod);
        Messenger.AddListener<int>(NormalEventType.ChangeCamera, ChangeCamera);

        //游戏结算
        Messenger.AddListener(NormalEventType.GameWin, GameWin);
        Messenger.AddListener(NormalEventType.GameLose, GameLose);

    }

    //移除事件监听
    private void RemoveLsitenr()
    {
        //UI窗口的操作
        Messenger.RemoveListener<string>(UIEventType.ShowPanel, ShowPanel);
        Messenger.RemoveListener(UIEventType.ClosePanelofTop, ClosePanelOfTop);
        Messenger.RemoveListener(UIEventType.CloseAllPanel, CloseAllPanel);

        Messenger.RemoveListener(UIEventType.OnNextOnClick, OnNextOnClick);
        Messenger.RemoveListener(UIEventType.OnReplayBtnOnClick, OnReplayBtnOnClick);

        Messenger.RemoveListener<bool>(NormalEventType.ShowJoystick, ShowJoystick);
        //相机的操作
        Messenger.RemoveListener<Transform>(NormalEventType.SetTarget, SetTarget);
        Messenger.RemoveListener<Transform>(NormalEventType.SetLookTarget, SetLookTarget);
        Messenger.RemoveListener<CinemachineBrain.UpdateMethod>(NormalEventType.SetUpdateMethod, SetUpdateMethod);
        Messenger.RemoveListener<int>(NormalEventType.ChangeCamera, ChangeCamera);

        //游戏结算
        Messenger.RemoveListener(NormalEventType.GameWin, GameWin);
        Messenger.RemoveListener(NormalEventType.GameLose, GameLose);
    }

}  