/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2022-04-02 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : Sigleton<JoystickManager>
{
    Joystick joystick;

    protected override void Awake()
    {
        base.Awake();
        joystick = GetComponent<Joystick>();
    }

    //显示手机摇杆
    public void ShowJoystick(bool isShow)
    {
        joystick.enabled = isShow;
        
    }


}
