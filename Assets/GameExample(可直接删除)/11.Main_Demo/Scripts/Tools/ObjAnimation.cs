/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-29 
 *说明:    基于DoTween的物体动画系统
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MyDemo
{
    public class ObjAnimation
    {
        /// <summary>
        /// 物体放大
        /// </summary>
        public void UpScale(Transform transform, float StartSize = 0, float size = 1.0f, float duration = 0.5f)
        {
            //如果该物体已经有Tween在播放，则打断
            if (DOTween.IsTweening(transform)) { return; }

            transform.gameObject.SetActive(true);
            transform.localScale = Vector3.one * StartSize;
            transform.DOScale(size, duration);
        }

        /// <summary>
        /// 物体缩小
        /// </summary>
        public void DownScale(Transform transform, float duration = 0.5f)
        {
            if (DOTween.IsTweening(transform)) { return; }

            transform.DOScale(0, duration).OnComplete(() => transform.gameObject.SetActive(false));
        }

        /// <summary>
        /// 物体自转
        /// </summary>
        public void DORotate_Self(Transform transform, float Speed = 1)
        {
            if (DOTween.IsTweening(transform)) { return; }

            transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y + 360, transform.localRotation.z), Speed, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
        }

        /// <summary>
        /// 清理物体身上的Tween
        /// </summary>
        public void ClearTween_Obj(Transform transform)
        {
            transform.DOKill();
        }


        //---------------按需拓展----------------------
    }
}

