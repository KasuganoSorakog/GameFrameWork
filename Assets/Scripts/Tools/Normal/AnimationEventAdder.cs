/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2022-04-02 
 *说明:    代码动态添加动画帧事件，应该在该脚本外做判断，
 * 一定要避免反复添加同一帧事件,或者在初始化的时候进行添加
 * 使用方法：
 *       AnimationClipAdder.Add(aa, "test", () => Debug.Log(11), 1f, "xx") ;
**/
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipAdder:MonoBehaviour
{
    List<Action> aniActionList = new List<Action>();

    public static void Add(Animator ani, string clipName, Action act, float time)
    {
        AnimationClipAdder adder = ani.gameObject.AddComponent<AnimationClipAdder>();
        adder.aniActionList.Add(act);

        AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            //根据动画名字找到需要添加的动画
            if (string.Equals(clips[i].name, clipName))
            {
                //添加动画事件
                AnimationEvent events = new AnimationEvent();
                events.functionName = "CallBack";
                events.intParameter = adder.aniActionList.Count - 1;
                events.time = time;
                clips[i].AddEvent(events);
                break;
            }
        }
        ani.Rebind();
    }

    void CallBack(int id)
    {
        aniActionList[id]?.Invoke();
    }
}
