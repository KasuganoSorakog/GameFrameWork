/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-06-09 
 *说明:    对象池管理器
 *          
**/  

using UnityEngine;  
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MyDemo
{
    public class PoolManager : MonoBehaviour
    {
        //通过对象池名字    ——>   找到对应的池子
        Dictionary<PoolName, ObjectPool> poolDic;

        //通过已经取出的对象 ——>  找到对应的池子
        Dictionary<GameObject, ObjectPool> instanceLookup;

        private void Awake()
        {
            poolDic = new Dictionary<PoolName, ObjectPool>();           //实例化对象池字典
            instanceLookup = new Dictionary<GameObject, ObjectPool>();  //实例化对象索引字典
        }



        //初始化对象池(Main) 在此处初始化并创建对象池
        public void Init()
        {
            //---------TODO：根据实际需要创建对象池-----------------
            //创建对象池，并添加进字典
            //CreatePool(PoolName.bulletPoll, null, 10, null, null);
        }

        //创建对应的对象池，游戏中每有一个新的对象池，都应该调用一次该函数(TODO: 如果后期还需要在场景加载时，动态实例化对象池，还需要进一步修改)
        //但是对象池的操作，都应该在对象池管理里。 如果有多个场景，需要加一个场景判断，根据不同的场景，实例化不同的对象池出来。。也可能是根据关卡level，也可能是根据当前Scence。
        public void CreatePool(PoolName poolName, GameObject preObject, int count, UnityAction actionGet = null, UnityAction actionRelease = null)
        {
            //1.首先创建一个空物体
            GameObject poolObj = new GameObject(EnumToString(poolName));
            //2.给池子设置父物体
            poolObj.transform.parent = transform;
            //3.在空物体上挂载对象池脚本
            poolObj.AddComponent<ObjectPool>();
            //4.对象池初始化(TODO: actionGet , actionRelease 一直为空，应该每个对象池都有自己的方法，比如设定父类等)
            ObjectPool thisPool = poolObj.GetComponent<ObjectPool>();
            thisPool.SetPool(EnumToString(poolName), preObject, count, false, actionGet, actionRelease);
            //5.记录进字典，方便查找
            poolDic.Add(poolName, thisPool);
        }


        //转换名字
        private string EnumToString(PoolName name)
        {
            return name.ToString();
        }

        #region 公开方法
        //从池子里面拿东西出来
        public GameObject GetObject(PoolName name)
        {
            return GetObject(name, Vector3.zero, Vector3.zero);
        }

        public GameObject GetObject(PoolName name, Vector3 position, Vector3 rotation)
        {
            if (!poolDic.ContainsKey(name))
            {
                Debug.Log("找不到你的对象池！请检查是否正确生成");
                return null;
            }

            GameObject clone = poolDic[name].GetObject();
            clone.transform.position = position;
            clone.transform.eulerAngles = rotation;
            //这个字典存储已经实例化出来的物体
            instanceLookup.Add(clone, poolDic[name]);
            return clone;
        }

        //释放返回池子
        public void ReleaseObject(GameObject clone)
        {
            //查找是不是对象池出来的东西。如果是就执行
            if (instanceLookup.ContainsKey(clone))
            {
                //释放返回池子
                instanceLookup[clone].ReleaseObject(clone);
                //从字典中移除
                instanceLookup.Remove(clone);
            }
            else
            {
                Debug.Log("找不到该物体对应的池子！请检查是否是池子里的物体");
            }
        }

        //清理对应池子(池子里面的内容全部清空)
        public void ClearPool(PoolName name)
        {
            poolDic[name].ClearPool();
        }
        #endregion

    }

    //池子的名字
    public enum PoolName
    {
        bulletPoll,
        monsterPool
    }
}
