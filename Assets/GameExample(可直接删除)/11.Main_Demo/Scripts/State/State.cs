/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:  状态的基类
**/

using System;
using UnityEngine;

namespace MyDemo
{
    abstract class State<T>
    {

        //当状态被进入时执行这个
        public virtual void Enter(T entity) { }
        //状态的主要内容
        public virtual void Excute(T entity) { }
        //当状态退出时执行
        public virtual void Exit(T entity) { }

        public virtual bool OnMessage(T entity, MessageType messageType, params object[] objs) { return false; }

    }
}
