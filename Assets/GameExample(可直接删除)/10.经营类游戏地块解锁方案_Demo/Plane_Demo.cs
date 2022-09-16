/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/
using MyDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class Plane_Demo : MonoBehaviour
    {
        [Header("地块编号")]
        public int Plane_Index;

        [Header("地块等级")]
        public int Plane_Level;

        //当前亮度值
        private float RampNum = 1.0f;

        private Material mat;

        //渐变方向参数
        private int dirVar = -1;

        //解锁触发器
        public GameObject[] Triggers;

        //是否开启闪烁效果
        public bool isOpenFlash;

        //物体动画库
        ObjAnimation objAni = new ObjAnimation();

        private void Awake()
        {
            mat = GetComponent<MeshRenderer>().material;
        }

        public void Update()
        {
            if (isOpenFlash)
            {
                if (RampNum >= 1.0f)
                {
                    dirVar = -1;
                }
                else if (RampNum <= 0.7f)
                {
                    dirVar = 1;

                }
                RampNum += dirVar * Time.deltaTime * 0.4f;
                FlashBlock(RampNum);
            }
        }

        //设置地块闪烁的效果
        public void FlashBlock(float num)
        {
            mat.SetFloat("_RampThreshold", num);
        }

        //解锁地块
        public void UnLockPlane()
        {
            isOpenFlash = false;
            BusinessClass_Demo_Main.Instance.m_gameData.m_gameDataSave.IsUnLockLevel[Plane_Index] = true;
            GetComponent<MeshRenderer>().material.SetFloat("_RampThreshold", 0.7f);
            GetComponent<NavMeshSourceTag>().enabled = true;
            ShowUnLockTriggerArea(false);
        }

        //显示解锁触发区域
        public void ShowUnLockTriggerArea(bool isShow)
        {
            for (int i = 0; i < Triggers.Length; i++)
            {
                if (isShow)
                {
                    objAni.UpScale(Triggers[i].transform, 0, 0.2f, 0.5f);
                }
                else
                {
                    objAni.DownScale(Triggers[i].transform);
                }
            }
        }
    }

}
