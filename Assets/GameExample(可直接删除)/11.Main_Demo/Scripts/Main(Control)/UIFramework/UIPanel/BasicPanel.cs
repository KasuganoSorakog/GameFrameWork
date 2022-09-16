/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-25 
 *说明:    游戏的基础面板(一直显示在最外层的)
**/  

using UnityEngine;  
using System.Collections;
using TMPro;
using System.Collections.Generic;

namespace MyDemo
{
    public class BasicPanel : BasePanel
    {
        //关卡等级框
        public TextMeshProUGUI text_Level;


        //当前分数
        public TextMeshProUGUI text_Score;

        //每一关的标题
        private Dictionary<int, string> TitleDic = new Dictionary<int, string>();

        private BasicPanelData basicPanelData;

        public void OnDestroy()
        {
            RemoveTitleDic();
        }

        public override void OnEnter()
        {
            base.OnEnter();

        }


        public override void OnPause() { base.OnPause(); }

        public override void OnResume() { base.OnResume(); }

        public override void OnExit() { base.OnExit(); }

        //添加标题进字典
        private void AddTitleDic()
        {
            TitleDic.Add(1, "Kill the man");
            TitleDic.Add(2, "Kill the old man");
            TitleDic.Add(3, "take the gas");
            TitleDic.Add(4, "Driving the car");
            TitleDic.Add(5, "Break Flower bottle");
        }

        //移除标题出字典
        private void RemoveTitleDic()
        {
            TitleDic.Remove(1);
            TitleDic.Remove(2);
            TitleDic.Remove(3);
            TitleDic.Remove(4);
            TitleDic.Remove(5);
        }

        //从字典中获取到对应的标题
        private string GetTitle(int num)
        {
            int temp;
            //如果关卡小于5,不变, 原值赋予
            if (num <= 5)
            {
                temp = num;
            }
            else
            {
                //如果求余等于0，则为第五关
                if (num % 5 == 0)
                {
                    temp = 5;
                }
                else
                {
                    //否则就取求余的值
                    temp = num % 5;
                }
            }
            return TitleDic.GetValue(temp);
        }

        //根据当前场景，初始化场景Level
        public void SetLevel()
        {
            text_Level.SetText("Level " + GameSaveManager.Load<int>(DataType.Level));
        }


        //刷新计分板
        public void Update_ScoreNum(int num)
        {
            //设置计分板
            text_Score.SetText(num.ToString());
        }

        //初始化所有关卡信息
        public void InitUIData()
        {
            AddTitleDic();
            ResetAllData();
        }

        //重置所有关卡信息
        public void ResetAllData()
        {
            Update_ScoreNum(0);
            SetLevel();
        }

        //设置PanelData
        public override void SetPanelData(UIPanelData uiPanelData)
        {
            basicPanelData = (BasicPanelData)uiPanelData;
        }
    }
}
