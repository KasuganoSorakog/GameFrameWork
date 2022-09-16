/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-29 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class Plane_Trigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(TagType.Player))
            {

                Plane_Demo plane = GetComponentInParent<Plane_Demo>();
                int plane_Level = plane.Plane_Level;
                int plane_Gold = BusinessClass_Demo_Main.Instance.m_gameData.GetUnLockGold(plane_Level);
                int player_Level = BusinessClass_Demo_Main.Instance.m_gameData.GetPlayer_Level();
                int player_Gold = BusinessClass_Demo_Main.Instance.m_gameData.GetPlayer_Gold();
                //1. 判断玩家的权限等级是否达到
                if (plane_Level <= player_Level + 1)
                {
                    //2. 判断玩家的金币是否足够
                    if (player_Gold >= plane_Gold)
                    {
                        //减少玩家金币
                        BusinessClass_Demo_Main.Instance.RemoveGold(plane_Gold);
                        //解锁地块
                        BusinessClass_Demo_Main.Instance.UnLockPlane(plane);
                        //判断是否应该增加玩家的权限等级
                        if (plane_Level == player_Level + 1)
                        {
                            BusinessClass_Demo_Main.Instance.m_gameData.Add_Player_Leve();
                        }
                    }
                }
            }
        }
    }
}

