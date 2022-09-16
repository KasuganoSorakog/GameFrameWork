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
using DG.Tweening;
using UnityEngine.UI;

namespace MyDemo
{
    public class StartPanel : BasePanel
    {

        public Image Img_drag;

        public Image Img_finger;

        private StartPanelData startPanelData;

        private UIAnimation anim = new UIAnimation();

        public override void Awake()
        {
            base.Awake();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            anim.UpScaleAndDownScel_Child(Img_drag.transform);
            anim.FingerAni_Child(Img_finger.transform);

        }

        public override void OnPause()
        {
            base.OnPause();

        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        //设置PanelData
        public override void SetPanelData(UIPanelData uiPanelData)
        {
            startPanelData = (StartPanelData)uiPanelData;
        }
    }
}
