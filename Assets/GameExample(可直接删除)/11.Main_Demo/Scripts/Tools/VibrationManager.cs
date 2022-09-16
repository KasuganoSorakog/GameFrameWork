/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-22 
 *说明:    手机震动管理器
 * 为了防止连续碰撞时，产生不和谐的效果, 应该设置一个间隔
**/

using UnityEngine;
using System.Collections;
using MoreMountains.NiceVibrations;

namespace MyDemo
{
    public static class VibrationManager
    {
        //游戏成功时候的震动
        public static void TriggerSuccess()
        {
            MMVibrationManager.Haptic(HapticTypes.Success);
        }

        //游戏失败时候的震动
        public static void TriggerFail()
        {
            MMVibrationManager.Haptic(HapticTypes.Failure);
        }

        //触发轻度震动
        public static void TriggerLight_Shock()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        //触发中度震动
        public static void TriggerMid_Shock()
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }

        //触发严重震动
        public static void TriggerGrave_Shock()
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
    }
}
