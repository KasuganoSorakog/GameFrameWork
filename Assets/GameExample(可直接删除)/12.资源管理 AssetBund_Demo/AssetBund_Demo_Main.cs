/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2022-03-31 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class AssetBund_Demo_Main : MonoBehaviour
    {
//*  使用方法:
 //* 在使用前，先自动化更新一轮AB包标签 之后就可以使用 ABType.xxxx 代替包名
 //*  ABManager.Instance.LoadRes<GameObject>("model.unity3d", "cube");   -- 同步加载
 //* ABManager.Instance.LoadResAsync<GameObject>("model.unity3d", "cube", UnityAction); -- 异步加载
 //*
 //* 卸载方法：
 //*  ABManager.Instance().UnLoadAsset(asset);
 //*  非依赖型：Resources.UnloadAsset(asset);   asset = null;
 //*  依赖型：Destroy(instObj); Resources.UnloadUnusedAssets();
    }
}

