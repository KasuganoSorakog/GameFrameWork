/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:  
 UI管理器, 通过GameMain.instance().m_UIManger获取   UI的核心
 UI的显示逻辑并非 ShowUI(窗口名)
 而是相当于一个容器，可以层层叠加。比如点击一下按钮显示A窗口， A窗口里面点击一下按钮显示B窗口。 B窗口里面点击一下显示C窗口
 退出时候，先退出C窗口，再退出B窗口，最后退出A窗口。这样做的好处就是，当一个界面中有多个窗口存在时，不用一个个点名去关(SetActive)。

 如何新增窗口？
 1.先在UICanvas下创建窗口预制体，之后保存到"Resources/UIPanel"中，创建一个继承于BasePanel的脚本，如TestPanel挂上；同时挂一个CanvasGroup组件
 2.修改 "Resources/UIpanel/UIPanelTypeJson" ，此Json文件负责保存脚本路径，添加上你的预制体路径与名字
 3.在UIPanelType 类中添加窗口名称的常量。
 4.使用GameMain.Instance().ShowPanel() 与ClosePanelOfTop() 来控制窗口显隐
 5.在TestPanel中初始化各种事件，如按钮绑定事件
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class UIManager : MonoBehaviour
    {
        //手机摇杆
        public Joystick m_Joystick;

        //Panel数据文件
        public UIPanelPathData m_uiPanelPathData;

        //获取到当前物体上的Canvas
        private Transform canvasTransform;
        private Transform CanvasTransform
        {
            get
            {
                if (canvasTransform == null)
                {
                    /**此处可以必须要改成你的Canvas的位置,否则会找不到Canvas**/
                    canvasTransform = GetComponent<Canvas>().transform;
                }
                return canvasTransform;
            }
        }
        //保存Panel对应的名字，预制体
        private Dictionary<string, GameObject> panelPreDict;

        //根据面板名称，获取对应面板(物体)         --窗口字典
        private Dictionary<string, BasePanel> panelDict;

        //使用栈
        private Stack<BasePanel> panelStack;

        private void Awake()
        {
            GetUIPanelPre_ScriptTable();
        }

        //显示窗口
        public void ShowPanel(string panelType, UIPanelData uiPanelData = null) { PushPanel(panelType, uiPanelData); }

        //关闭顶部窗口
        public void ClosePanelOfTop() { PopPanel(); }

        //关闭所有窗口
        public void CloseAllPanel()
        {
            //如果当前栈为空，则创建一个新栈出来
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0)
            {
                return;
            }
            int currentCount = panelStack.Count;
            //调用所有窗口的退出方法
            for (int i = 0; i < currentCount; i++)
            {
                //退出栈顶面板
                BasePanel topPanel = panelStack.Pop();
                //调用退出方法
                topPanel.OnExit();
            }
        }

        //入栈方法(显示窗口)主要过程：通过字典访问到对应的窗口对象(如果没有，则生成),新建一个窗口放到Canvas下面
        private void PushPanel(string panelType, UIPanelData uiPanelData = null)
        {
            //如果当前栈为空，则创建一个新栈出来
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            //停止上一个画面
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }
            BasePanel panel = GetPanel(panelType, uiPanelData);
            panelStack.Push(panel);
            //调用进入方法
            panel.OnEnter();
        }

        //出栈方法
        private void PopPanel()
        {
            //如果当前栈为空，则创建一个新栈出来
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0)
            {
                return;
            }
            //退出栈顶面板
            BasePanel topPanel = panelStack.Pop();
            //调用退出方法
            topPanel.OnExit();
            //恢复到上一个面板
            if (panelStack.Count > 0)
            {
                BasePanel panel = panelStack.Peek();
                //调用重新唤醒方法
                panel.OnResume();
            }
        }

        //获取UI的窗口
        public BasePanel GetPanel(string panelType, UIPanelData uiPanelData = null)
        {
            //如果窗口字典为空，则重新创建一个字典
            if (panelDict == null)
            {
                panelDict = new Dictionary<string, BasePanel>();
            }
            //从窗口预制体字典中找到该窗口（如果该窗口被销毁，返回肯定是空的；如果没有被销毁，则会返回具体对象）
            BasePanel panel = panelDict.GetValue(panelType);
            //如果没有实例化面板，寻找路径进行实例化，并且存储到已经实例化好的字典面板中
            if (panel == null)
            {
                GameObject obj = panelPreDict.GetValue(panelType);
                GameObject panelGo = Instantiate(obj, CanvasTransform, false);
                panelGo.name = obj.name;
                panel = panelGo.GetComponent<BasePanel>();
                //如果有数据传入，则设置数据
                if (uiPanelData.IsNotNull())
                {
                    panel.SetPanelData(uiPanelData);
                }
                panelDict.Add(panelType, panel);
            }
            return panel;
        }

        //从ScriptTable中获取到各个Panel预制体的信息
        private void GetUIPanelPre_ScriptTable()
        {
            panelPreDict = new Dictionary<string, GameObject>();

            //此处原本从Resouce中读取文件
            //UIPanelPathData uiPanelPathData = Resources.Load<UIPanelPathData>("Data/UIPanelPathData");
            //由于是Demo，先简单粗暴直接public 变量拖进去，正式项目可略微调整
            UIPanelPathData uiPanelPathData = m_uiPanelPathData;

            foreach (var panelData in uiPanelPathData.panelData)
            {
                panelPreDict.Add(panelData.panelName, panelData.panelPre);
            }
        }



        //显示手机摇杆
        public void ShowJoystick()
        {
            m_Joystick.gameObject.SetActive(true);
            m_Joystick.background.gameObject.SetActive(false);
        }

        //关闭手机摇杆
        public void CloseJoystick()
        {
            m_Joystick.background.gameObject.SetActive(false);
        }
    }
}
