/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    Messenger.cs即是基于中介者模式进行开发 消息处理工具类
 *使用方法：
 *   Messenger.Add<int,int,float....>("YourMethods", YourMethods);
 *   Messenger.Remove("YourMethods");
 *   Messenger.Brocast("YourMethods",int,int,float...);
 *   
**/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

static internal class Messenger
{
    public delegate void CallBack();
    public delegate void CallBack<T>(T arg1);
    public delegate void CallBack<T, U>(T arg1, U arg2);
    public delegate void CallBack<T, U, V>(T arg1, U arg2, V arg3);

    //游戏开始时自动创建MessengerHelper
    //static private MessengerHelper messengerHelper = (new GameObject("MessengerHelper")).AddComponent<MessengerHelper>();

    static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    static public List<string> permanentMessages = new List<string>();

    static public void MarkAsPermanent(string eventType)
    {
        permanentMessages.Add(eventType);
    }

    static public void Cleanup()
    {
        List<string> messagesToRemove = new List<string>();
        foreach (KeyValuePair<string, Delegate> pair in eventTable)
        {
            bool wasFound = false;
            foreach (string message in permanentMessages)
            {
                if (pair.Key == message)
                {
                    wasFound = true;
                }
            }
            if (wasFound)
            {
                messagesToRemove.Add(pair.Key);
            }
        }
        foreach (string message in messagesToRemove)
        {
            eventTable.Remove(message);
        }
    }

    static public void PrintEventTable()
    {
        Debug.Log("==== PrintEventTable ====");

        foreach (KeyValuePair<string, Delegate> pait in eventTable)
        {
            Debug.Log("\t\t\t" + pait.Key + "\t\t\t" + pait.Value);
        }
    }

    static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, null);
        }
        Delegate d = eventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }

    static public void OnLisenerRemoving(string eventType, Delegate listenerBeingRemoved)
    {
        if (eventTable.ContainsKey(eventType))
        {
            Delegate d = eventTable[eventType];

            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
        }
        else
        {
            throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
        }
    }

    static public void OnListenerRemoved(string eventType)
    {
        if (eventTable[eventType] == null)
        {
            eventTable.Remove(eventType);
        }
    }

    static public void OnBrodcasting(string eventType)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }
    }

    static public BroadcastException CreateBroadcastSignatureException(string eventType)
    {
        return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
    }

    public class BroadcastException : Exception
    {
        public BroadcastException(string msg) : base(msg) { }
    }

    public class ListenerException : Exception
    {
        public ListenerException(string msg) : base(msg) { }
    }

    //增加监听
    //无参数
    static public void AddListener(string eventType, CallBack handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (CallBack)eventTable[eventType] + handler;
    }
    //一个参数
    static public void AddListener<T>(string eventType, CallBack<T> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (CallBack<T>)eventTable[eventType] + handler;
    }
    //两个参数
    static public void AddListener<T, U>(string eventType, CallBack<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (CallBack<T, U>)eventTable[eventType] + handler;
    }
    //三个参数
    static public void AddListener<T, U, V>(string eventType, CallBack<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (CallBack<T, U, V>)eventTable[eventType] + handler;
    }

    //移除监听
    //无参数
    static public void RemoveListener(string eventType, CallBack handler)
    {
        OnLisenerRemoving(eventType, handler);
        eventTable[eventType] = (CallBack)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    //一个参数
    static public void RemoveListener<T>(string eventType, CallBack<T> handler)
    {
        OnLisenerRemoving(eventType, handler);
        eventTable[eventType] = (CallBack<T>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    //两个参数
    static public void RemoveListener<T, U>(string eventType, CallBack<T, U> handler)
    {
        OnLisenerRemoving(eventType, handler);
        eventTable[eventType] = (CallBack<T, U>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    //三个参数
    static public void RemoveListener<T, U, V>(string eventType, CallBack<T, U, V> handler)
    {
        OnLisenerRemoving(eventType, handler);
        eventTable[eventType] = (CallBack<T, U, V>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //广播事件
    //无参数
    static public void Broadcast(string eventType)
    {
        OnBrodcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            CallBack callback = d as CallBack;

            if (callback != null)
            {
                callback();
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
    //一个参数
    static public void Broadcast<T>(string eventType, T arg1)
    {
        OnBrodcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callback = d as CallBack<T>;
            if (callback != null)
            {
                callback(arg1);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
    //两个参数
    static public void Broadcast<T, U>(string eventType, T arg1, U arg2)
    {
        OnBrodcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, U> callback = d as CallBack<T, U>;
            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
    //三个参数
    static public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        OnBrodcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, U, V> callback = d as CallBack<T, U, V>;
            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
}
