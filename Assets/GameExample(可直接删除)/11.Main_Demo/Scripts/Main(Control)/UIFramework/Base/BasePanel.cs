/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:  窗口的基类
**/

using UnityEngine;

namespace MyDemo
{
    public class BasePanel : MonoBehaviour
    {
        //规定每一个窗口都挂一个 CanvasGroups组件
        //通过canvasGroup.alpha来控制面板的显隐
        //canvasGroup.blocksRaycasts控制窗口是否能够被点击
        //当然也可以进行 SetActive()进行控制，相对而言，该方法更方便快捷
        protected CanvasGroup canvasGroup;

        public virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        //窗口开始时
        public virtual void OnEnter()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        //窗口停止时  
        public virtual void OnPause()
        {
            canvasGroup.blocksRaycasts = false;
        }

        //窗口重新被唤醒时  
        public virtual void OnResume()
        {
            canvasGroup.blocksRaycasts = true;
        }

        //窗口退出时
        public virtual void OnExit()
        {
            //gameObject.SetActive(false);
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        //设置Panel数据
        public virtual void SetPanelData(UIPanelData uiPanelData)
        {

        }
    }

}
