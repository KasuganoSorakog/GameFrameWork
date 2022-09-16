/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-31 
 *说明:    随机名字生成器
 * 使用方法： RanDomName.GetName();
 * 修改方法： 可在LastName()中编排好可能随机到的名字
**/

using UnityEngine;  
using System.Collections;
using System;

public static class RandomName 
{
    //从预先编制好的名字数组中随机取出
    public static string GetName()
    {
        return LastName[UnityEngine.Random.Range(0, LastName.Length)];
    }

    //随机凑几个英文字母拼凑起来
    public static string GetName_Chaos()
    {
        string getName = "";

        for (int i = 0; i < UnityEngine.Random.Range(3, 7); i++)
        {
            getName += RangeName(UnityEngine.Random.Range(0, 2));
        }

        return getName + " " + LastName[UnityEngine.Random.Range(0, LastName.Length)];          //随机取得名字,并返回
    }

    private static string RangeName(int i)
    {
        string tmpString = "";

        switch (i)
        {
            case 0:
                tmpString = createBigAbc();
                break;
            case 1:
                tmpString = createSmallAbc();
                break;
        }
        return tmpString;
    }

    /// <summary>
    /// 生成单个大写随机字母
    /// </summary>
    private static string createBigAbc()
    {
        //A-Z的 ASCII值为65-90
        //UnityEngine.Random random = new UnityEngine.Random();
        int num = UnityEngine.Random.Range(65, 91);
        string abc = Convert.ToChar(num).ToString();
        return abc;
    }

    /// <summary>
    /// 生成单个小写随机字母
    /// </summary>
    private static string createSmallAbc()
    {
        //a-z的 ASCII值为97-122
        //UnityEngine.Random random = new UnityEngine.Random();
        int num = UnityEngine.Random.Range(97, 123);
        string abc = Convert.ToChar(num).ToString();
        return abc;
    }

    /// <summary>
    /// 英文 姓氏
    /// </summary>
    private static string[] LastName = {"William", "Wesley", "Warren", "Vincent", "Tony", "Tom", "Terence", "Stanley",
                                "Samuel" , "Spark" , "Stanley", "Sammy", "Rock", "Richard", "Randy","Quentin",
                                "Patrick","Peter","Phoebe","Matthew","Marcus","Leonard","Lawrence","Leo","Leopold",
                                "Larry","Justin","John","Johnny","Joseph","Keith","Jacob","Jeffery","Jerry","Jim",
                                "Jack","Jackson","Henry","Howard","Harrison","Hugo","Glendon","George","Garfield",
                                "Gabriel","Franklin","Francis","Edward","Donald","David","Cosmo","Christian","Carl"};
}  