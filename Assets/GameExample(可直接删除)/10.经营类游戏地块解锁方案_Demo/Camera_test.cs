/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    摄影机管理器
 * 相机的状态逻辑和其他的有所不同，是一种小型的状态模式
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class Camera_test : MonoBehaviour
    {

        //相机的状态
        private CameraState_test currentState;

        //用来存储合适的相机位
        Vector3 m_desirePosition;

        //用来存储平滑移动到的人物位置
        Vector3 m_smoothedPosition;

        [Header("相机与人物的距离")]
        //距离相机与人物的距离
        public Vector3 m_offset;

        [Header("相机的角度")]
        public Vector3 m_CameraAngel;                 //镜头的角度

        Transform m_target;

        [Header("相机的平滑速度"), Range(0, 3)]
        public float m_smoothedSpeed;

        void Start()
        {
            currentState = CameraState_test.Normal;
        }

        void LateUpdate()
        {
            //判断跟随的目标是否为空
            if (m_target != null)
            {
                //需要启动相机时，把该注释去掉，状态机自会运行
                StateMachineRun(currentState);
            }
        }

        //设置当前相机的状态(TODO：需要改变状态直接切即可, 该简易版的状态机缺点是，没有设计状态进入时操作，状态退出时操作，如果需要，可以加两个委托参数，进行方法调用)
        public void SetState(CameraState_test cameraState)
        {
            currentState = cameraState;
        }

        //相机的简易版状态机  TODO: 可能还有其他状态需要添加
        private void StateMachineRun(CameraState_test state)
        {
            switch (state)
            {
                case CameraState_test.Normal:
                    NorMalState();
                    break;
                case CameraState_test.Fixed:
                    Fixed();
                    break;
                case CameraState_test.CloseUp:
                    CloseUp();
                    break;
                default:
                    break;
            }
        }

        //相机的正常状态 TODO:
        public void NorMalState()
        {
            //计算合适的位置
            m_desirePosition = m_target.position + m_offset;
            //相机平滑调整到人物位置 
            m_smoothedPosition = Vector3.Lerp(transform.position, m_desirePosition, m_smoothedSpeed);
            //改变相机的位置
            transform.position = m_desirePosition;
            //改变相机的角度
            transform.rotation = Quaternion.Euler(m_CameraAngel);

        }

        //相机的固定状态(设置成这个状态就不跟随移动了，可能还有其他操作）
        public void Fixed() { }

        //相机的特写状态(游戏中可能需要把摄影机通过某种移动的方式，移动到某个地方观察，就要进入到这个状态，此时的游戏中其他操作应该是禁止的)
        public void CloseUp()
        {

        }

        //初始化相机
        public void Init(Transform obj)
        {
            SetTarget(obj);
        }

        //设置相机跟随目标
        public void SetTarget(Transform obj)
        {
            m_target = obj;
        }


    }

    public enum CameraState_test
    {
        Normal,
        Fixed,
        CloseUp
    }

}
