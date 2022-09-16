/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    游戏失败窗口
**/  

using UnityEngine;  
using System.Collections;
using UnityEngine.UI;

namespace MyDemo
{
    public class FailPanel : BasePanel
    {

        public Button btn_Replay;                   //重新开始游戏

        private FailPanelData failPanelData;

        UIAnimation ani = new UIAnimation();

        public override void Awake()
        {
            base.Awake();
            btn_Replay.onClick.AddListener(OnReplayBtnOnClick);

        }

        public void OnDestroy()
        {
            btn_Replay.onClick.RemoveListener(OnReplayBtnOnClick);

        }

        public override void OnEnter()
        {
            base.OnEnter();
            ani.HeartBeatAni_Child(btn_Replay.transform);

        }

        public override void OnPause() { base.OnPause(); }

        public override void OnResume() { base.OnResume(); }

        public override void OnExit() { base.OnExit(); }

        //重新开始游戏按钮回调
        private void OnReplayBtnOnClick()
        {
            Messenger.Broadcast(EventType.OnReplayBtnOnClick);
        }

        //设置PanelData
        public override void SetPanelData(UIPanelData uiPanelData)
        {
            failPanelData = (FailPanelData)uiPanelData;
        }
    }
}
