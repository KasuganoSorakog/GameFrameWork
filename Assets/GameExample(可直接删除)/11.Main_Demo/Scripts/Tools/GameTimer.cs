/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:       Sora 
 *Unity版本：2019.4.9f1 
 *日期:         2021-05-21 
 *说明:  
 使用方法： 
 GameTimer gameTime = new GameTime();
 gameTime.set(时间);
 if(gameTime.IsEndble() && gameTime.IsTimeOut()){ 你的语句;}
**/

using UnityEngine;
using System.Collections;

namespace MyDemo
{
    public class GameTimer
    {
        //计时时长
        float m_duration = 0;
        //开始计时时间
        float m_startTime = 0;
        //开关
        bool m_enable = true;
        //单次或循环
        bool m_doOnce = true;
        //是否暂停
        private bool m_isPause = false;
        //暂时时的进度时间
        private float m_progressInPause = -1;

        //设置时长
        public void Set(float dur)
        {
            m_duration = dur;
            m_startTime = Time.time;
            m_enable = true;
            if (m_isPause)
            {
                m_isPause = false;
                m_progressInPause = -1;
            }
        }

        //设置单次循环
        public void SetDoOnce(bool b)
        {
            m_doOnce = b;
        }

        //暂停
        public void SetPause(bool b)
        {
            if (m_isPause != b)
            {
                m_isPause = b;
                if (b)//开启暂停
                {
                    m_progressInPause = GetTimeProgress(); //记下刚开启暂停的进度
                }
                else//结束暂停
                {
                    m_startTime = Time.time - m_progressInPause;//重置为正确的开始时间
                    m_progressInPause = -1;
                }
            }
        }

        //判断时间抵达
        public bool IsTimeOut()
        {
            if (m_enable)
            {
                if (m_isPause)
                {
                    return false;
                }
                else
                {
                    if (Time.time - m_startTime > m_duration)
                    {
                        if (!m_doOnce)
                            m_startTime = Time.time;

                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return true;
        }

        //重新计时
        public void ReCount()
        {
            m_startTime = Time.time;
        }

        //获取进度百分比
        public float GetTimePercent()
        {
            if (m_enable)
            {
                if (m_isPause)
                {
                    return (m_progressInPause - m_startTime) / m_duration;
                }
                else
                {
                    return (Time.time - m_startTime) / m_duration;
                }
            }
            else
            {
                return 0;
            }
        }

        //获取进度时间
        public float GetTimeProgress()
        {
            //暂停中，且暂停进度储存，返回上次暂停的进度时间
            if (m_isPause && m_progressInPause != -1)
            {
                return m_progressInPause;
            }
            else
            {
                if (m_enable)
                    return (Time.time - m_startTime);
                else
                    return 0;
            }
        }

        //获取剩余时间
        public float GetTimeLeft()
        {
            if (m_enable)
            {
                if (m_isPause)
                {
                    return m_duration - m_progressInPause;
                }
                else
                {
                    return m_duration - (Time.time - m_startTime);
                }
            }
            else
            {
                return 0;
            }
        }

        //开关
        public void SetEnable(bool b)
        {
            m_enable = b;
        }

        //增加时间
        public void Add(float dur)
        {
            m_duration += dur;
        }

        //获取开关
        public bool GetEnable()
        {
            return m_enable;
        }
    }

}




