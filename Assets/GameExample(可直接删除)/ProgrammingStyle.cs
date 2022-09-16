/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-08-19 
 *说明:    此脚本主要约束开发时候《编码规范》, 不参与游戏逻辑
 *         具体项目可以删除
**/  

using UnityEngine;  
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ProgrammingStyle : MonoBehaviour 
{
    #region Basic App
    //2. 前缀最好展示所属的Component类型，比如 Button > Btn
    Button btnEnterMainPage;
    static int count;

    int curSelectIndex = 0;

    //3. public 类型使用首字母大写驼峰式
    public int lastIndex = 0;

    //4. public 类型属性也算public类型变量
    public int CurSelectIndex
    {
        get { return curSelectIndex; }
    }

    //5.临时变量
    private void Start()
    {
        btnEnterMainPage = transform.Find("BtnEnterMainPage").GetComponent<Button>();
        //临时变量命名采用首字母小写驼峰式
        GameObject firstPosGo = transform.Find("FirstPosGo").gameObject;

    }

    //6.方法名一律首字母大写驼峰式
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Advanced

    //_____常用组件命名______
    //GameObject -> Go
    GameObject TestGo;
    //Transform -> Trans
    Transform  TestTrans;
    //Dictionary -> Dic
    Dictionary<int, int> m_TestDic;
    //_____统计数量的命名_____
    //Num 表示数量
    int TestNum;
    //index 表示下标序号
    int curSelectIndexx;
    //Count 表示计数
    int testCount;
    //_________________________
    //Current -> Cur
    float curTest;
    //Image -> Img (UI组件)
    Image img_Test;
    //Button -> Btn (UI组件)
    Button btn_Test;
    //Material -> Mat(材质球)
    Material mat_Test;

    //__________________________
    //Pos -> Position -> Vector3
    Vector3 cachedPos;

    //Rot -> Rotation -> Vector3
    Vector3 cachedRot;

    //Size -> Vector2
    Vector2 cachedSize;

    //后缀s表示数组
    Vector3[] cachedPositions;

    //List类型的话，用List作为后缀
    List<Vector3> cachedPosList;
    #endregion

    //---------------------------------
    //网络连接通常有 Connecting, Connected, Connect表示三种状态

}

//接口名称一般以大写I作为前缀
public interface IConvertible
{ 

}

//自定义的属性以Attribute结尾
public class TableAttribute:Attribute
{ 

}

//自定义异常以Exception结尾
public class NullEmptyException: Exception
{

}

//代码分块，对于模块化功能，可以使用 #region....#endregion

//尽量避免用var命名!!! 增加维护难度
//像这种代码，完全看不懂是什么东西↓
//var resultModel = testList.Exist(t => t.Index == 7);

