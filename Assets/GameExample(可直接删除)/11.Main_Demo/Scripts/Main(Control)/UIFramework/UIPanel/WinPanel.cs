/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    
**/  

using UnityEngine;  
using System.Collections;
using UnityEngine.UI;

namespace MyDemo
{
    public class WinPanel : BasePanel
    {
        public Button btn_Next;                                 //开始下一关

        public Button btn_Replay;                               //重新开始游戏

        public Image img_Title;                                 //标题图片

        private WinPanelData winPanelData;

        UIAnimation anim = new UIAnimation();

        public override void Awake()
        {
            base.Awake();
            //给按钮添加监听
            btn_Next.onClick.AddListener(OnNextBtnOnclick);
            btn_Replay.onClick.AddListener(OnReplayBtnOnClick);
        }


        public void OnDestroy()
        {
            //给按钮添加监听
            btn_Next.onClick.RemoveListener(OnNextBtnOnclick);
            btn_Replay.onClick.RemoveListener(OnReplayBtnOnClick);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            anim.UpScale_Child(img_Title.transform);
        }

        public override void OnPause() { base.OnPause(); }

        public override void OnResume() { base.OnResume(); }

        public override void OnExit() { base.OnExit(); }

        //重新开始游戏按钮回调
        private void OnReplayBtnOnClick()
        {
            Messenger.Broadcast(EventType.OnReplayBtnOnClick);
        }

        //进入下一关游戏按钮回调
        private void OnNextBtnOnclick()
        {
            Messenger.Broadcast(EventType.OnNextOnClick);
        }

        //设置PanelData
        public override void SetPanelData(UIPanelData uiPanelData)
        {
            winPanelData = (WinPanelData)uiPanelData;
        }
    }
}
