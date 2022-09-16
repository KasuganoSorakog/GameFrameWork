/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2021-11-01 
 *说明:    
**/
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

class CinemaChineManager : Sigleton<CinemaChineManager>
{
    //当前相机的组件
    private CinemachineBrain m_cinemachineBrain;

    //当前存在的虚拟相机
    public CinemachineVirtualCamera[] scence_Camera = null;

    //当前正在执行的相机编号
    int m_index = 0;

    public void Init()
    {
        m_cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    //切换相机
    public void ChangeCamera(int index)
    {
        //scence_Camera.AsParallel().ForAll(p => p.enabled = false);
        for (int i = 0; i < scence_Camera.Length; i++)
        {
            scence_Camera[i].enabled = false;
        }
        scence_Camera[index].enabled = true;
        m_index = index;
    }

    //设置相机跟随目标
    public void SetTarget(Transform obj)
    {
        scence_Camera[m_index].Follow = obj;
    }

    //设置相机观察目标
    public void SetLookTarget(Transform obj)
    {
        scence_Camera[m_index].LookAt = obj;
    }

    //设置相机的跟随模式
    public void SetUpdateMethod(CinemachineBrain.UpdateMethod method)
    {
        m_cinemachineBrain.m_UpdateMethod = method;
    }

    //重置数据
    public void ResetData()
    {

    }
}
