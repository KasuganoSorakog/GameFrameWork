/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-25 
 *说明:    
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    class Demo_State_2 : State<StateMachine_Demo_Main>
    {


        //当状态被进入时执行这个
        public override void Enter(StateMachine_Demo_Main entity) 
        {
            Debug.Log("Hello...我进入了 第二 状态！");
        }
        //状态的主要内容
        public override void Excute(StateMachine_Demo_Main entity)
        {
            Debug.Log("Hello...我在 第二 状态反复执行");
        }
        //当状态退出时执行
        public override void Exit(StateMachine_Demo_Main entity) 
        {
            Debug.Log("Hello... 我退出了 第二 的状态");
        }

    }
}
