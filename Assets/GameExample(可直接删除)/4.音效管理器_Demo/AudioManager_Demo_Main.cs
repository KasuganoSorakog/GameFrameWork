/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-25 
 *说明:    
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class AudioManager_Demo_Main : MonoBehaviour
    {

        void Start()
        {
            //播放音乐
            GameAudioManager.Instance().PlayLoopAudio(GameAudioType.test1);

            //播放音效
            //GameAudioManager.Instance().PlayOnceAudio(GameAudioType.test1);
        }

    }

}
