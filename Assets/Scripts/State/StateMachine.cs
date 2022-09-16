/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:  状态机脚本
**/

using System.Collections.Generic;

class StateMachine<T>
{
    //当前状态机所属的物体
    T m_pOwner;
    //当前状态
    State<T> m_pCurrentState;
    //智能体处于上一个状态的记录
    State<T> m_pPreviousState;
    //每次FSM被更新时，这个状态逻辑被调用
    State<T> m_pGlobalState;

    //状态字典
    Dictionary<int, State<T>> stateDic = new Dictionary<int, State<T>>();

    //初始化FSM有限状态机
    public StateMachine(T owner)
    {
        m_pOwner = owner;
        m_pCurrentState = null;
        m_pPreviousState = null;
        m_pGlobalState = null;
    }

    //使用这些方法来初始化FSM, 并执行一次进入方法(Enter)  通常不要用这个来转换状态，应该使用ChangeState()，否则不会经过Excute()退出事件
    public void SetCurrentState(int stateTag) 
    {
        State<T> s = stateDic.GetValue(stateTag);
        m_pCurrentState = s; 
        m_pCurrentState.Enter(m_pOwner); 
    }

    public void SetGlobalState(int stateTag) 
    {
        State<T> s = stateDic.GetValue(stateTag);
        m_pGlobalState = s; 
        m_pGlobalState.Enter(m_pOwner); 
    }

    public void SetPreviousState(int stateTag) 
    {
        State<T> s = stateDic.GetValue(stateTag);
        m_pPreviousState = s; 
        m_pGlobalState.Enter(m_pOwner); 
    }

    //增加状态
    public void AddState(int stateTag, State<T> s)
    {
        if (!stateDic.ContainsKey(stateTag))
        {
            stateDic.Add(stateTag, s);
        }
    }
    
    //调用这个来更新FSM有限状态机
    public void OnUpdate()
    {
        //如果一个全局状态存在，调用它的执行方法，实时监测有什么突然加进来的事件
        if (m_pGlobalState != null)
        {
            m_pGlobalState.Excute(m_pOwner);
        }
        //对当前的状态相同
        if (m_pCurrentState != null)
        {
            m_pCurrentState.Excute(m_pOwner);
        }
    }

    //改变到一个新的状态,根据实体调用它自身的Excute, Enter方法  State<T> pNewState
    public void ChangeState(int state)
    {
        State<T> pNewState = stateDic.GetValue(state);
        if (AssertState(pNewState))
        {
            //保留前一个状态的记录
            m_pPreviousState = m_pCurrentState;
            //调用现有的状态的退出方法
            m_pCurrentState.Exit(m_pOwner);
            //改变一个状态到新的状态
            m_pCurrentState = pNewState;
            //调用新状态的进入方法
            m_pCurrentState.Enter(m_pOwner);
        }
    }

    //改变状态回到前一个状态
    public void RevertToPreviousState()
    {
        int preState = -1;
        foreach (var item in stateDic)
        {      
            if (item.Value == m_pPreviousState)
            {
                preState = item.Key;
            }
        }
        ChangeState(preState);
    }

    //获取当前状态，等待状态，全局状态    
    public int CurrentState() 
    {
        int s = stateDic.GetKey(m_pCurrentState);
        return s; 
    }

    public int GlobalState() 
    {
        int s = stateDic.GetKey(m_pGlobalState);
        return s;
    }

    public int PreviousState() 
    {
        int s = stateDic.GetKey(m_pPreviousState);
        return s;
    }

    //判断状态是有效的
    public bool AssertState(State<T> newState)
    {
        //当新状态不为空，且不为当前状态.
        if (newState != null && newState != stateDic[CurrentState()])
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
