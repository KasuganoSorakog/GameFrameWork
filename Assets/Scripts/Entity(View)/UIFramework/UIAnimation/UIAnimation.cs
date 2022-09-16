/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-24 
 *说明:    UI的动画库 0.1
 * 可根据项目需求进行效果扩充
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public class UIAnimation 
{
    Dictionary<string, Tween> tweenDic = new Dictionary<string, Tween>();

    //----------------------------------适用于UI面板的模块-------------------------------------------------
    /// <summary>
    /// UIPanel渐渐隐藏(用于退出窗口) 
    /// </summary>
    /// <param name="transform"></param>
    public void FadeOut(Transform transform, float duration = 0.5f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.DOFade(0, duration);
    }

    /// <summary>
    /// UIPanel渐渐显示(用于显示窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void FadeIn(Transform transform, float duration = 0.5f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        canvas.alpha = 0;
        canvas.blocksRaycasts = true;
        canvas.DOFade(1, duration);
    }

    /// <summary>
    /// UIPanel右移(用于退出窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void MoveOut(Transform transform, Vector2 vec2, float duration = 0.3f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;
        canvas.blocksRaycasts = false;

        transform.DOLocalMove(vec2, duration).OnComplete(() => canvas.alpha = 0);
    }

    /// <summary>
    /// UIPanel左移(用于显示窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void MoveIn(Transform transform, Vector2 vec2, float duration = 0.3f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        canvas.alpha = 1;
        canvas.blocksRaycasts = true;

        transform.localPosition = vec2;
        transform.DOLocalMove(Vector2.zero, duration);
    }

    /// <summary>
    /// UIPanel缩小(用于退出窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void DownScale(Transform transform, float duration = 0.3f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        canvas.blocksRaycasts = false;
        transform.DOScale(0, duration).OnComplete(() => canvas.alpha = 0);
    }

    /// <summary>
    /// UIPanel放大(用于显示窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void UpScale(Transform transform,float size = 0.5f, float duration = 0.3f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        canvas.blocksRaycasts = false;
        transform.localScale = Vector3.zero;
        transform.DOScale(size, duration);
    }

    /// <summary>
    /// 逐渐放大，并从屏幕右边移动出来
    /// </summary>
    public void UpScaleAndMove(Transform transform, float duration = 0.5f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        transform.localScale = Vector3.zero;
        transform.DOScale(1, duration);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, duration);
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
    }

    /// <summary>
    /// 逐渐缩小，并且移动到屏幕右边
    /// </summary>
    /// <param name="transform"></param>
    public void DownScaleAndMove(Transform transform, float duration = 0.5f)
    {
        if (DOTween.IsTweening(transform)) return;
        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if (canvas == null) return;

        transform.DOScale(0, duration);
        transform.DOLocalMoveX(1000, duration).OnComplete(() => { canvas.alpha = 1; canvas.blocksRaycasts = true; });
    }

    //---------------------------------------------------------------------------------------------------------------


    //-----------------------------------------适用于UI元件的模块----------------------------------------------------
    /// <summary>
    /// UIPanel渐渐隐藏淡出(用于显示窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void FadeOut_Child(Transform transform)
    {
        if (DOTween.IsTweening(transform)) return;

        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();

        if (canvas == null) return;

        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.DOFade(0, 0.5f).OnComplete(
            () =>
                {
                    transform.gameObject.SetActive(false);
                    canvas.alpha = 1;
                });
    }

    /// <summary>
    /// UIPanel渐渐显示(用于显示窗口)
    /// </summary>
    /// <param name="transform"></param>
    public void FadeIn_Child(Transform transform)
    {
        if (DOTween.IsTweening(transform)) return;

        CanvasGroup canvas = transform.GetComponent<CanvasGroup>();
        if(canvas == null) return; 

        transform.gameObject.SetActive(true);
        canvas.alpha = 0;
        canvas.blocksRaycasts = true;
        canvas.DOFade(1, 0.5f);
    }

    /// <summary>
    /// 放大UI的子物体
    /// </summary>
    /// <param name="transform"></param>
    public void UpScale_Child(Transform transform, float duration = 0.3f)
    {
        if (DOTween.IsTweening(transform)) return;

        transform.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, duration);
    }

    /// <summary>
    /// 缩小UI的子物体
    /// </summary>
    /// <param name="transform"></param>
    public void DownScale_Child(Transform transform, float duration = 0.3f)
    {
        if (DOTween.IsTweening(transform)) return;

        transform.DOScale(0, duration).OnComplete(
            () =>
                {
                    transform.gameObject.SetActive(false);
                    transform.localScale = Vector2.one;
                }
            );
    }

    /// <summary>
    /// UI反复放大缩小的效果(持续)
    /// </summary>
    public void UpScaleAndDownScel_Child(Transform transform, float maxSize = 1.5f, float minSize = 1.2f, float duration = 1)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(maxSize, duration)).Append(transform.DOScale(minSize, duration)).SetLoops(-1).SetEase(Ease.Linear);
    }

    /// <summary>
    /// UI组件移出
    /// </summary>
    /// <param name="transform">目标物体</param>
    /// <param name="vec2">     初始位置</param>
    /// <param name="transform">到达时间</param>
    public void MoveOut_Chilid(Transform transform, Vector2 vec2 , float duration = 0.4f)
    {
        if (DOTween.IsTweening(transform)) return;

        transform.DOScale(0, duration);
        transform.DOLocalMove(vec2, duration).OnComplete(() =>
            {
                transform.gameObject.SetActive(false);
                transform.localPosition = Vector2.zero;
                transform.localScale = Vector2.one;
            });
    }

    /// <summary>
    /// UI组件移入
    /// </summary>
    /// <param name="transform">目标物体</param>
    /// <param name="vec2">     初始位置</param>
    /// <param name="transform">到达时间</param>
    public void MoveIn_Chilid(Transform transform, Vector2 vec2 , float duration = 0.4f)
    {
        if (DOTween.IsTweening(transform)) return;

        transform.localScale = Vector3.zero;
        transform.DOScale(1, duration);
        transform.gameObject.SetActive(true);
        transform.localPosition = vec2;
        transform.DOLocalMove(Vector2.zero, duration);
    }

    /// <summary>
    /// UI心跳的动画效果
    /// </summary>
    public void HeartBeatAni_Child(Transform transform, float duration = 0.5f)
    {
        if (DOTween.IsTweening(transform)) return;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.2f, duration)).Append(transform.DOScale(1.0f, duration)).SetEase(Ease.OutQuint).Append(transform.DOScale(1.0f, 1f)).SetLoops(-1);
    }

    /// <summary>
    /// 开始界面手指转圈圈
    /// </summary>
    public void FingerAni_Child(Transform transform)
    {
        if (tweenDic.ContainsKey(transform.name))
            tweenDic[transform.name].Kill();
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(new Vector3(120,-94),0.25f)).Append(transform.DOLocalMove(new Vector3(5, -20), 0.25f)).Append(transform.DOLocalMove(new Vector3(-96, 53), 0.25f)).Append(transform.DOLocalMove(new Vector3(-168, -12), 0.25f)).Append(transform.DOLocalMove(new Vector3(-100, -98), 0.25f)).Append(transform.DOLocalMove(new Vector3(8, -17), 0.25f)).Append(transform.DOLocalMove(new Vector3(117, 58), 0.25f)).Append(transform.DOLocalMove(new Vector3(188, -14), 0.25f)).SetLoops(-1).SetEase(Ease.Linear);
        tweenDic.Add(transform.name, seq);
    }

    /// <summary>
    /// 数字跳动效果， 如果用的是TextMeshPro , 则将Text改一下
    /// 使用案例：
    /// uiAnima.JumpNum_Child(suvNum,  DataManager.Instance.PlayerSaveData.ResourceData.suvi, 1);
    /// </summary>
    /// <param name="text">Text文本组件</param>
    /// <param name="StartNum">开始数字</param>
    /// <param name="targetNum">目标数字</param>
    /// <param name="duration">时间</param>
    public void JumpNum_Child(Text text, int StartNum, int targetNum, int duration, Action action = null)
    {
        //判断上一个DoTween是否在执行
        if (tweenDic.ContainsKey(text.name))
            tweenDic[text.name].Kill();

        int temp = int.Parse(text.text);

        Tween JumpTween;
        JumpTween = DOTween.To(() => temp, x =>
        {
            temp = x;
            text.text = temp.ToString();
        }, targetNum, duration).
        OnComplete(() =>
        {
            action?.Invoke();
            tweenDic.Remove(text.name);
        });
        if (!tweenDic.ContainsKey(text.name))
        {
            tweenDic.Add(text.name, JumpTween);
        }
    }

    //跳动效果
    public void JumpNum_Obj(Transform obj)
    {
        obj.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).OnComplete(() => obj.transform.DOScale(new Vector3(1, 1, 1), 0.1f));
    }

    //延时执行函数
    public void DeleToRun(float duration, Action action = null)
    {
        int timeOut = 0;
        DOTween.To(() => timeOut, a =>
        {
            timeOut = a;
        }, 1, duration).OnComplete(() => { action?.Invoke(); });
    }

    //清除并关闭某个动画
    public void CloseAniOnKey(string str)
    {
        tweenDic[str].Kill();
        tweenDic.Remove(str);
    }
}
