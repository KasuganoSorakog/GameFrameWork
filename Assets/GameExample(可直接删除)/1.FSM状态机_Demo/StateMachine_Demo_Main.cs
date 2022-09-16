/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-25 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public enum StateType
    {
        Demo_State_1,
        Demo_State_2,
        Demo_State_3
    }

    public class StateMachine_Demo_Main : MonoBehaviour
    {
        //声明状态机
        StateMachine<StateMachine_Demo_Main> FSM_stateMachine;

        //声明状态
        private Demo_State_1 Demo_State_1;

        private Demo_State_2 Demo_State_2;

        private Demo_State_3 Demo_State_3;

        private Dictionary<StateType, State<StateMachine_Demo_Main>> stateDic;

       
        private void Start()
        {
            //初始化状态机
            FSM_stateMachine = new StateMachine<StateMachine_Demo_Main>(this);

            //初始化字典
            stateDic = new Dictionary<StateType, State<StateMachine_Demo_Main>>();

            //初始化状态
            Demo_State_1 = new Demo_State_1();

            Demo_State_2 = new Demo_State_2();

            Demo_State_3 = new Demo_State_3();

            stateDic.Add(StateType.Demo_State_1, Demo_State_1);

            stateDic.Add(StateType.Demo_State_2, Demo_State_2);

            stateDic.Add(StateType.Demo_State_3, Demo_State_3);

            //设置第一个进入的状态
            FSM_stateMachine.SetCurrentState(stateDic.GetValue(StateType.Demo_State_1));
        }


        private void Update()
        {
            //接下来让状态机自己跑， 只需要在 状态类 内修改要执行的内容即可。
            if (FSM_stateMachine != null)
            {
                FSM_stateMachine.Update();
            }
        }

        //切换到第一状态
        public void Btn_Change_State1()
        {
            ChangeState(StateType.Demo_State_1);
        }

        //切换到第二状态
        public void Btn_Change_State2()
        {
            ChangeState(StateType.Demo_State_2);
        }

        //切换到第三状态
        public void Btn_Change_State3()
        {
            ChangeState(StateType.Demo_State_3);
        }

        //改变当前游戏状态
        public void ChangeState(StateType gameState)
        {
            if (stateDic.GetValue(gameState) != null)
            {
                FSM_stateMachine.ChangeState(stateDic.GetValue(gameState));
            }
        }

        //------------------------------------- 替代写法 ->  if..else..  或者 switch..case... --------------------------

        //用if...else...来写的话，则等价于下面内容
        //Demo_State_1， Demo_State_2， Demo_State_3 所代替的就是 if..else if..中间那一大块内容，相当于分离了出来，如果使用if写法，则整个状态机会超过200行代码
        //优点：内容分块处理，清晰明了..     缺点： 需要建立多几个状态文件..
        //private int m_state = 0;

        //private void FixedUpdate()
        //{
        //    if (m_state == 0)
        //    {

        //    }
        //    else if (m_state == 1)
        //    {

        //    }
        //    else if (m_state == 3)
        //    {

        //    }
        //    else if(m_state == 4)
        //    {

        //    }
        //}
    }
}
