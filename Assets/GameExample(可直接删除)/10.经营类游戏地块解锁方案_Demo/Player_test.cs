/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MyDemo
{
    public class Player_test : MonoBehaviour
    {

        private NavMeshAgent m_agent;

        //虚拟遥感
        public Joystick m_joystick;



        public void Awake()
        {
            m_agent = GetComponent<NavMeshAgent>();

        }

        public void Update()
        {
            //拖拽的时候反复发射射线检测地面；判断是否符合解锁内容
            if (Mathf.Abs(m_joystick.Horizontal) > 0.1f || Mathf.Abs(m_joystick.Vertical) > 0.1f)
            {
                PlayerMove();
            }
        }

        //人物移动
        public void PlayerMove()
        {
            //获取方向
            Vector3 temVec3 = new Vector3(m_joystick.Horizontal, 0, m_joystick.Vertical);
            temVec3 = temVec3.normalized;

            Vector2 dir = new Vector2(m_joystick.Horizontal, m_joystick.Vertical);
            float m_angle = Vector2.Angle(dir, Vector2.up);
            Vector3 cross = Vector3.Cross(Vector2.up, dir);

            if (cross.z > 0)
            {
                m_angle = 360 - m_angle;
            }
            Quaternion targetRotation = Quaternion.Euler(0, m_angle, 0) * Quaternion.identity;
            m_agent.velocity = temVec3 * 5;
            transform.rotation = targetRotation;
        }
    }

}
