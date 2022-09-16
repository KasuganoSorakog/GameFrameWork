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

namespace MyDemo
{
    public class CinemaChineManager : MonoBehaviour
    {

        //当前相机的组件
        private CinemachineBrain m_cinemachineBrain;

        //当前存在的虚拟相机
        public CinemachineVirtualCamera[] Scence_Camera = null;

        //当前正在执行的相机编号
        int m_index = 0;

        private void Awake()
        {
            m_cinemachineBrain = transform.GetComponent<CinemachineBrain>();
        }


        //设置相机跟随目标
        //切换相机
        public void ChangeCamera(int index)
        {
            for (int i = 0; i < Scence_Camera.Length; i++)
            {
                Scence_Camera[i].enabled = false;
            }

            Scence_Camera[index].enabled = true;
            m_index = index;
        }

        //设置相机跟随目标
        public void SetTarget(Transform obj)
        {
            Scence_Camera[m_index].Follow = obj;
        }

        //设置相机观察目标
        public void SetLookTarget(Transform obj)
        {
            Scence_Camera[m_index].LookAt = obj;
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

}
