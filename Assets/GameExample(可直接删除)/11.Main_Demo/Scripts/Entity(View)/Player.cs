/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    玩家所挂的脚本，负责控制玩家的行为
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyDemo 
{
    class Player : MonoBehaviour
    {
        /*玩家的固定数据可以写在这里，玩家的动态数据，如速度等，可以写在DataManger,可以动态调整*/

        //------------行走相关------------------
        //摇杆方向
        private Vector2 dir;

        //摇杆角度
        private float m_angle;

        //存储旋转的角度
        private Quaternion targetRotation;

        //存储上一次位移的方向值
        private Vector3 m_lastDic;

        //存储上一次旋转的方向值
        private Quaternion m_lastTurn;

        //转弯速度[数值越大，转得越快]
        private float turnSpeed = 6;

        //是否能够移动(游戏管理器应该传递消息给玩家个体，来使玩家移动或停止)
        [HideInInspector]
        public bool CanMove = false;
        //------------------------------------------
        //玩家的速度倍率
        private float SpeedMul = 1;

        //玩家的真实模型
        public GameObject Player_Mod = null;
        
        //虚拟摇杆
        [HideInInspector]
        public Joystick m_joystick = null;

        //玩家的状态机
        StateMachine<Player> FSM_stateMachine;

        //上一次播放的动画
        string m_LastTrriger;

        //自身的Animator
        Animator m_Animator;

        //自身的渲染器
        Renderer m_render;

        //自身的角色控制器
        CharacterController m_controller;

        //角色状态字典
        Dictionary<PlayerState, State<Player>> playerStateDic;

        //人物的站立状态
        private PlayerIdleState playerIdleState;

        //人物的走路状态
        private PlayerWalkState playerWalkState;

        //人物的跑步状态
        private PlayerRunState playerRunState;

        //人物的胜利状态
        private PlayerWinState playerWinState;

        //玩家的死亡状态
        private PlayerDieState playerDieState;

        //在构造函数中初始化
        public Player()
        {
            //初始化状态机
            FSM_stateMachine = new StateMachine<Player>(this);
        }

        private void Awake()
        {
            playerStateDic = new Dictionary<PlayerState, State<Player>>();
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (FSM_stateMachine != null)
            {
                FSM_stateMachine.Update();
            }
        }

        private void Init()
        {
            //初始化人物状态
            InitState();
            //初始化人物动画机    
            m_Animator = GetComponent<Animator>();
            //初始化人物的渲染器
            m_render = GetComponentInChildren<Renderer>();

            //判断是否获取成功，否则，继续找子物体获取
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            //初始化转弯系数
            turnSpeed = GameManager.Instance.m_DataManger.turnSpeed;
            //初始化角色控制器
            m_controller = GetComponent<CharacterController>();
            //初始化人物摇杆
            m_joystick = GameObject.Find("UICanvas/Floating Joystick").GetComponent<Joystick>();
            //设置人物的当前状态
            SetCurrentState(PlayerState.Idle);
        }

        //播放角色动画
        public void PlayAnimation(string triggleName)
        {
            //当上一次播放的动画不为空时,重置上次的Trriger
            if (!string.IsNullOrEmpty(m_LastTrriger))
            {
                m_Animator.ResetTrigger(m_LastTrriger);
            }
            //设置本次动画的TriggleName
            m_Animator.SetTrigger(triggleName);
            m_LastTrriger = triggleName;
        }

        //初始化各种状态  TODO
        private void InitState()
        {
            playerIdleState = new PlayerIdleState();
            playerWalkState = new PlayerWalkState();
            playerRunState = new PlayerRunState();
            playerDieState = new PlayerDieState();
            playerWinState = new PlayerWinState();

            playerStateDic.Add(PlayerState.Idle, playerIdleState);
            playerStateDic.Add(PlayerState.Walk, playerWalkState);
            playerStateDic.Add(PlayerState.Run, playerRunState);
            playerStateDic.Add(PlayerState.Win, playerWinState);
            playerStateDic.Add(PlayerState.Die, playerDieState);
        }

        //初始化游戏当前状态
        private void SetCurrentState(PlayerState playerState)
        {
            if (playerStateDic.GetValue(playerState) != null)
            {
                FSM_stateMachine.SetCurrentState(playerStateDic.GetValue(playerState));
            }
        }

        //改变游戏状态
        public void ChangeState(PlayerState playerState)
        {
            if (playerStateDic.GetValue(playerState) != null)
            {
                FSM_stateMachine.ChangeState(playerStateDic.GetValue(playerState));
            }
        }

        //玩家的行走方法(刚体方式)
        //public void PlayerMove()
        //{
        //    //一、改变当前人物的位置
        //    Vector3 temVec3 = new Vector3(m_joystick.Horizontal, 0, m_joystick.Vertical);
        //    //接收成为上一次位移(方向)信息
        //    m_lastDic = temVec3.normalized;
        //    //当角色的角度变化完了之后，才开始朝着变化的方向移动,否则还是按当前方向移动
        //    if (Quaternion.Angle(targetRotation, transform.rotation) < 10)
        //    {
        //        float x = m_lastDic.x * GameManager.Instance().m_DataManger.PlyaerSpeed_walk * SpeedMul;
        //        float y = m_rigidbody.velocity.y;
        //        float z = m_lastDic.z * GameManager.Instance().m_DataManger.PlyaerSpeed_walk * SpeedMul;
        //        m_rigidbody.velocity = new Vector3(x, y, z);
        //    }
        //    else
        //    {
        //        float x = transform.forward.x * GameManager.Instance().m_DataManger.PlyaerSpeed_walk * SpeedMul;
        //        float y = m_rigidbody.velocity.y;
        //        float z = transform.forward.z * GameManager.Instance().m_DataManger.PlyaerSpeed_walk * SpeedMul;
        //        m_rigidbody.velocity = new Vector3(x, y, z);
        //    }
        //    //二、改变人物的旋转    
        //    dir = new Vector2(m_joystick.Horizontal, m_joystick.Vertical);
        //    m_angle = Vector2.Angle(dir, Vector2.up);
        //    Vector3 cross = Vector3.Cross(Vector2.up, dir);
        //    if (cross.z > 0)
        //    {
        //        m_angle = 360 - m_angle;
        //    }
        //    targetRotation = Quaternion.Euler(0, m_angle, 0) * Quaternion.identity;
        //    //接收成为上一次旋转信息
        //    m_lastTurn = targetRotation;
        //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);//利用Slerp插值让物体进行旋转  5是旋转速度 越大旋转越快
        //    if (Quaternion.Angle(targetRotation, transform.rotation) < 1)
        //    {
        //        transform.rotation = targetRotation;
        //    }
        //}
        //public void PlayerMove()
        //{
        //    //一、改变当前人物的位置
        //    Vector3 temVec3 = new Vector3(m_joystick.Horizontal, 0, m_joystick.Vertical);
        //    //接收成为上一次位移(方向)信息
        //    m_lastDic = transform.TransformDirection(temVec3.normalized);
        //    m_controller.Move(new Vector3(m_lastDic.x, 0, m_lastDic.z) * Time.deltaTime * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul);

        //    //二、改变人物的旋转    
        //    dir = new Vector2(m_joystick.Horizontal, m_joystick.Vertical);
        //    m_angle = Vector2.Angle(dir, Vector2.up);
        //    Vector3 cross = Vector3.Cross(Vector2.up, dir);
        //    if (cross.z > 0)
        //    {
        //        m_angle = 360 - m_angle;
        //    }
        //    targetRotation = Quaternion.Euler(0, m_angle, 0) * Quaternion.identity;
        //    //接收成为上一次旋转信息
        //    m_lastTurn = targetRotation;
        //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);//利用Slerp插值让物体进行旋转  5是旋转速度 越大旋转越快
        //    if (Quaternion.Angle(m_lastTurn, transform.rotation) < 1)
        //    {
        //        transform.rotation = targetRotation;
        //    }

        //}

        //玩家的行走方式(角色控制器)
        public void PlayerMove()
        {
            //一、改变当前人物的位置
            Vector3 temVec3 = new Vector3(m_joystick.Horizontal, 0, m_joystick.Vertical);
            //接收成为上一次位移(方向)信息
            m_lastDic = temVec3.normalized;
            //当角色的角度变化完了之后，才开始朝着变化的方向移动,否则还是按当前方向移动
            //if (Quaternion.Angle(targetRotation, Player_Mod.transform.rotation) < 50)
            //{
            //    float x = m_lastDic.x * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul;
            //    float y = m_lastDic.y - 12f;
            //    float z = m_lastDic.z * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul;
            //    m_controller.Move(new Vector3(x, y, z) * Time.deltaTime);
            //}
            //else
            //{
            //    float x = transform.forward.x * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul;
            //    float y = m_lastDic.y - 12f;
            //    float z = transform.forward.z * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul;
            //    m_controller.Move(new Vector3(x, y, z) * Time.deltaTime);
            //}
            float x = m_lastDic.x * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul;
            float y = m_lastDic.y - 12f;
            float z = m_lastDic.z * GameManager.Instance.m_DataManger.PlyaerSpeed_walk * SpeedMul;
            m_controller.Move(new Vector3(x, y, z) * Time.deltaTime);
            //二、改变人物的旋转    
            dir = new Vector2(m_joystick.Horizontal, m_joystick.Vertical);
            m_angle = Vector2.Angle(dir, Vector2.up);
            Vector3 cross = Vector3.Cross(Vector2.up, dir);
            if (cross.z > 0)
            {
                m_angle = 360 - m_angle;
            }
            targetRotation = Quaternion.Euler(0, m_angle, 0) * Quaternion.identity;

            //接收成为上一次旋转信息
            m_lastTurn = targetRotation;
            Player_Mod.transform.rotation = Quaternion.Lerp(Player_Mod.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);//利用Slerp插值让物体进行旋转  5是旋转速度 越大旋转越快
            if (Quaternion.Angle(m_lastTurn, transform.rotation) < 1)
            {
                Player_Mod.transform.rotation = targetRotation;
            }
        }

        //设置玩家的速度倍率
        public void SetSpeedMul_Player(float num)
        {
            SpeedMul = num;
        }

        //按特定的路线转送消息到各个状态
        public bool HandleMessage(MessageType messageType, params object[] objs)
        {
            return FSM_stateMachine.HandleMessage(messageType, objs);
        }

    }
}
