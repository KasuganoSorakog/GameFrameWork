/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-09 
 *说明:    具体的对象池类(子弹，生物，NPC，特效)，凡是一切需要动态实例化出来的物体
 *        可以想象成一个箩筐，你需要拿果子的时候，就从里面拿一个出来
 *        拿出来的这个东西，应该放到场景中一起保存, 也就是gameWorld里面，如创造出一个NPC，则把该NPC也放进到 ScenInfo.cs 的列表里面， 
 *        该列表保存场景中所有的物体信息，统一进行管理
 *        当场景中的物体不用时候，应该考虑放回对象池里，也就是释放资源
 *        该类不需要去管，所有用到对象池的，直接在PoolManager中去管理即可
**/  

using UnityEngine;  
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MyDemo
{
    public class ObjectPool : MonoBehaviour
    {
        //当前对象池的名字
        private string poolName;

        //对象池的数量，需要生成多少个数量
        private int pooledAmount;

        //对象池的存储结构 -- 队列（当然List, Stack也可以） 
        private Queue<GameObject> pooledObject = new Queue<GameObject>();

        //对象池的预制体
        private GameObject ObjPrefabs;

        //获取对象时候执行的方法委托
        private UnityAction m_ActionOnGet;

        //释放对象时候的方法委托
        private UnityAction m_ActionOnRelease;

        //是否锁定对象池大小(锁定之后不可动态扩增) 通常情况可以不锁
        private bool lockPoolSize = false;

        /// <summary>
        /// 设置此对象池的一些基本属性
        /// </summary>
        /// <param name="name">对象池的名字</param>
        /// <param name="obj">对象池内的物体</param>
        /// <param name="amount">对象池内物体的数量</param>
        /// <param name="isLockSize">是否锁池子(锁住池子数量就定死了，不锁池子数量不够时候可以动态添加)</param>
        /// <param name="actionGet">对象池取出对象时的方法回调</param>
        /// <param name="actionRelease">对象池释放对象时的方法回调</param>
        /// 
        public void SetPool(string name, GameObject obj, int amount, bool isLockSize, UnityAction actionGet = null, UnityAction actionRelease = null)
        {
            poolName = name;                                            //初始化对象池的名字
            ObjPrefabs = obj;                                           //初始化对象的预制体
            pooledAmount = amount;                                      //初始化数量
            m_ActionOnGet = actionGet;                                  //取出对象时候的方法
            m_ActionOnRelease = actionRelease;                          //释放对象时候的方法
            lockPoolSize = isLockSize;                                  //是否可以锁池子

            WarmPool();                                                 //池子预热, 创建并隐藏物体
        }


        //1. 预热对象池子(生成物体，设为隐藏)
        private void WarmPool()
        {
            for (int i = 0; i < pooledAmount; i++)
            {
                CreateObj();
            }
        }

        //2. 从池子里面获得一个物体(公开)
        public GameObject GetObject()
        {
            //首先要判断里面还有没有东西,再判断池子是否锁定数量，之后再进行动态扩充池子
            if (pooledObject.Count == 0 && !lockPoolSize)
            {
                CreateObj();
            }
            //当前池子为空，且池子已经锁定数量
            if (pooledObject.Count == 0 && lockPoolSize)
            {
                return null;
            }

            GameObject nextObject = pooledObject.Dequeue();             //在队列的头部读取和删除一个元素，出队操作
            nextObject.SetActive(true);
            if (m_ActionOnGet != null)
            {
                m_ActionOnGet();
            }
            return nextObject;
        }

        //3. 释放一个物体返回池子(公开)
        public void ReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.parent = transform;                           //把物体挂回当前对象池
            pooledObject.Enqueue(obj);                                  //入队操作
            if (m_ActionOnRelease != null)
            {
                m_ActionOnRelease();                                    //释放时候的回调 (数据重置)
            }
        }

        //4. 清理池子(公开)
        public void ClearPool()
        {
            for (int i = 0; i < pooledObject.Count; i++)
            {
                //取出对象，并销毁...当然也可以直接把池子直接摧毁，就不用for循环了
                Destroy(pooledObject.Dequeue());
            }
        }

        //5. 获取对象池的名字(公开)
        public string GetPoolName()
        {
            return poolName;
        }

        //-----------创建物体(设置为隐藏)------------
        private void CreateObj()
        {
            GameObject newObj = Instantiate(ObjPrefabs);            //生成一个物体
            newObj.name = ObjPrefabs.name;                          //确保名字一致，防止(clone)
            newObj.transform.SetParent(transform);                  //设置当前所挂物体为父类
            newObj.SetActive(false);                                //设置隐藏
            pooledObject.Enqueue(newObj);                           //放入栈中, 需要创建对象的时候，直接从队列里面拿出来用即可  
        }
    }
}

