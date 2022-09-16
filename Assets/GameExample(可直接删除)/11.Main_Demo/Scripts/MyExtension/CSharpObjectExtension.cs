/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2021-11-04 
 *说明:    基本数据类型转Obj数组
**/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public static class CSharpObjectExtension
    {
        /// <summary>
        /// 功能：将任意数据类型转换成Obeject[]返回
        /// 示例：
        /// <code>
        /// int temp = 1;
        /// Object[] objs = temp.ToObjs();
        /// 此时返回一个Object[]
        /// </code>
        /// </summary>
        /// <param name="selfObj"></param>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <returns>是否为空</returns>
        public static object[] ToObjs<T>(this T x)
        {
            object[] objs = new object[1];
            objs[0] = x;
            return objs;
        }

        /// <summary>
        /// 判断该int数值是否为0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsZero(this int x)
        {
            if (x == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该int数值是否非0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsNotZero(this int x)
        {
            if (x != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该float数值是0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsZero(this float x)
        {
            if (x == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该float数值是否非0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsNotZero(this float x)
        {
            if (x != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该double数值是0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsZero(this double x)
        {
            if (x == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该double数值是否非0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsNotZero(this double x)
        {
            if (x != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 功能：判断是否为空
        /// 示例：
        /// <code>
        /// var simpleObject = new object();
        ///
        /// if (simpleObject.IsNull()) // 等价于 simpleObject == null
        /// {
        ///     // do sth
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj">判断对象(this)</param>
        /// <typeparam name="T">对象的类型（可不填）</typeparam>
        /// <returns>是否为空</returns>
        public static bool IsNull<T>(this T selfObj) where T : class
        {
            return selfObj == null;
        }


        /// <summary>
        /// 功能：判断不是为空
        /// 示例：
        /// <code>
        /// var simpleObject = new object();
        ///
        /// if (simpleObject.IsNotNull()) // 等价于 simpleObject != null
        /// {
        ///    // do sth
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj">判断对象（this)</param>
        /// <typeparam name="T">对象的类型（可不填）</typeparam>
        /// <returns>是否不为空</returns>
        public static bool IsNotNull<T>(this T selfObj) where T : class
        {
            return null != selfObj;
        }

        public static void DoIfNotNull<T>(this T selfObj, Action<T> action) where T : class
        {
            if (selfObj != null)
            {
                action(selfObj);
            }
        }

        /// <summary>
        /// 表达式成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Do(this bool selfCondition, Action action)
        {
            if (selfCondition)
            {
                action();
            }

            return selfCondition;
        }

        /// <summary>
        /// 是否相等
        /// 
        /// 示例：
        /// <code>
        /// if (this.Is(player))
        /// {
        ///     ...
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Is(this object selfObj, object value)
        {
            return selfObj == value;
        }
        /// <summary>
        ///  字典中获取数值的拓展, 通常获取字典的值是 dic["键名"],  或者dic.TryGetValue()
        ///  现在可以使用dic.getValue("键值")啦;
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="Tvalue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value = default(Tvalue);
            dict.TryGetValue(key, out value);
            return value;
        }
    }

}
