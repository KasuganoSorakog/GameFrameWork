/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    存档读档功能
 *         主要功能用法：
 *         GameSaveManager.Save<int>("test", 1);
 *         GameLoad
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    public static class GameSaveManager
    {
        //存储游戏数据
        public static void Save<T>(string dataType, T value)
        {
            if (typeof(T).Name.Equals("Int32"))
            {
                PlayerPrefs.SetInt(dataType, (int)(object)value);
            }
            else if (typeof(T).Name.Equals("Single"))
            {
                PlayerPrefs.SetFloat(dataType, (float)(object)value);
            }
            else if (typeof(T).Name.Equals("String"))
            {
                PlayerPrefs.SetString(dataType, (string)(object)value);
            }
            else
            {
                Debug.Log("存储的数据类型有误！只能存储int, float, string");
            }
        }

        //读取游戏数据
        public static T Load<T>(string dataType)
        {
            if (typeof(T).Name.Equals("Int32"))
            {
                //如果数据不存在，先存个1进去
                if (!PlayerPrefs.HasKey(dataType))
                {
                    Save<T>(dataType, (T)(object)1);
                    return (T)(object)PlayerPrefs.GetInt(dataType);
                }
                else
                {
                    return (T)(object)PlayerPrefs.GetInt(dataType, 0);
                }
            }
            else if (typeof(T).Name.Equals("Single"))
            {
                //如果数据不存在，先存个1进去
                if (!PlayerPrefs.HasKey(dataType))
                {
                    Save<T>(dataType, (T)(object)1);
                    return (T)(object)PlayerPrefs.GetFloat(dataType);
                }
                else
                {
                    return (T)(object)PlayerPrefs.GetFloat(dataType);
                }
            }
            else if (typeof(T).Name.Equals("String"))
            {
                //如果数据不存在，先存个1进去
                if (!PlayerPrefs.HasKey(dataType))
                {
                    Save<T>(dataType, (T)(object)1);
                    return (T)(object)PlayerPrefs.GetString(dataType);
                }
                else
                {
                    return (T)(object)PlayerPrefs.GetString(dataType);
                }
            }
            else
            {
                Debug.Log("存储的数据类型有误！只能存储int, float, string");
                return (T)(object)null;
            }
        }

    }

    public class DataType
    {
        public const string Level = "Level";

    }
}
