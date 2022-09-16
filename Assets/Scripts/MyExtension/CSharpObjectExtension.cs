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

public static class CSharpObjectExtension
{
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

    /// <summary>
    /// 从字典中通过值获取键, 但是要确保键是唯一的
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Tkey GetKey<Tkey, Tvalue>(this Dictionary<Tkey,Tvalue> dic, Tvalue value)
    {
        foreach (var item in dic)
        {
            if (item.Value.Equals(value))
            {
                return item.Key;
            }
        }
        return default(Tkey);
    }

    /// <summary>
    /// 从指定的List中删除另一个List的元素
    /// </summary>
    /// <typeparam name="T">列表的类型</typeparam>
    /// <param name="list">列表本身</param>
    /// <param name="sourceList">需要删除的内容</param>
    public static void Exclusive<T>(this List<T> list, List<T> sourceList)
    {
        List<T> exclusives = new List<T>();
        foreach (T anObject in list)
        {
            if (!sourceList.Contains(anObject))
            {
                exclusives.Add(anObject);
            }
        }
        //清空所有元素
        list.Clear();
        list.AddRange(exclusives);
    }

    /// <summary>
    /// 删除所有重复项
    /// </summary>
    /// <typeparam name="T">列表的类型</typeparam>
    /// <param name="list">列表本身</param>
    /// <returns></returns>
    public static List<T> Distinct<T>(this List<T> list)
    {
        List<T> returnList = new List<T>();
        foreach (T item in list)
        {
            if (!returnList.Contains(item))
            {
                returnList.Add(item);
            }
        }
        return returnList;
    }
}
