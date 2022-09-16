/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-30 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class ObjAnimation_Demo_Main : MonoBehaviour
    {

        ObjAnimation objAni = new ObjAnimation();

        public GameObject TargetObj;

        //放大物体尺寸
        public void UpScale_Obj()
        {
            objAni.UpScale(TargetObj.transform, 0, 1f, 0.5f);
        }

        //缩小物体尺寸
        public void DownScale_Obj()
        {
            objAni.DownScale(TargetObj.transform, 0.5f);
        }

        //物体自转
        public void DORotate_Self()
        {
            objAni.DORotate_Self(TargetObj.transform);
        }

        //清理物体身上的Tween
        public void ClearTween_Obj()
        {
            objAni.ClearTween_Obj(TargetObj.transform);
        }
    }

}
