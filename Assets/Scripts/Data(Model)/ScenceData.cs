/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:         用来保存场景内的信息
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceData :MonoBehaviour
{
    //当前场景编号(该值是唯一，且不可重复的)读取场景的时候，根据该编号进行信息加载
    [Header("当前场景编号")]
    public int ScenceIndex;

    //此处可以放入场景预制体中本来有的物体，可以在做预制体的时候顺便挂上去
    
}
