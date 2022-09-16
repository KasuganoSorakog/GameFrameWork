/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.16f1 
 *日期:         2022-03-30 
 *说明:    AB包资源管理器    
 *  使用方法:
 *  在使用前，先自动化更新一轮AB包标签 之后就可以使用 ABType.xxxx 代替包名
 *  ABManager.Instance.LoadRes<GameObject>("model.unity3d", "cube");   -- 同步加载
 *  ABManager.Instance.LoadResAsync<GameObject>("model.unity3d", "cube", UnityAction); -- 异步加载
 *  
 *  卸载方法：
 *  ABManager.Instance().UnLoadAsset(asset);
 *  非依赖型：Resources.UnloadAsset(asset);   asset = null;
 *  依赖型：Destroy(instObj);  Resources.UnloadUnusedAssets();
 *  ----------------------------------------------------------------------
 *  初始化： 使用的时候务必要确认读取的路径
 *  打包： 使用AssetBundles Browser第三方插件打包
 *  加载(原理)： 
 *     加载资源原理分两步，
 *     1. 加载资源包；               AssetBundle.LoadFromFile  
 *     2. 从资源包里面提取所需资源   AssetBundle.LoadAsset
 *  卸载:  卸载的思路根据资源的使用情况，
 *      如果是非依赖型Asset 如贴图，文本，shader , 
 *      第一步： 调用 ABManager.Instance().UnLoadAsset();  该过程只对包进行了卸载检测，并未卸载资源
 *      第二步： ①卸载贴图/文本/shader等非依赖型Asset   Resources.UnloadAsset(asset);
 *               ②卸载依赖型asset，如prefab,材质等， 无法调用Resources.UnloadAsset(asset)来卸载，会报错
 *               应该 Destroy(instObj);  Resources.UnloadUnusedAssets();    注意，回收空闲资源这步比较耗时，应该在合适时候调用
 *     
 *     当然也可以直接 ABManager.instance().UnLoadAB("")   直接卸载包后，资源会成为裸资源，需要找机会也一并卸载掉
 *     
 *   卸载AB包分两种情况：   
 *        ①卸载资源的同时，检测是否应该卸载包..(根据包内资源的使用情况)  ABManager.Instance().UnLoadAsset(assetName);
 *        ②卸载包的时候，保留资源（相当于加载完毕后，卸载包） ABManager.Instance().UnLoadAB(abName);  
 *        看情况选择，如果加载完资源后，不需要这个包了，可以把包卸载掉；如果短期内还需要到的话，则先不进行卸载
 *        ③卸载资源的时候，即便是最后一个，也不对包进行卸载 ABManager.Instance().UnLoadAsset(assetName,false);
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyDemo
{
    public class ABManager : Sigleton<ABManager>
    {
        //主包
        private AssetBundle mainAb = null;

        //依赖包获取用的配置文件
        private AssetBundleManifest mainfest = null;

        //AB包不能够重复加载，重复加载会报错；所以需要建立字典来存储加载过的AB包(已加载的AB包缓存池)
        private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

        //每个AB包的引用表
        private Dictionary<string, string[]> dependencies = new Dictionary<string, string[]>();

        //每个AB包的引用列表 键： AssetBundle的包名， List该包所引用的资源  引用计数
        //在AB包中的资源被使用时，引用计数 + 1，没有被使用时，应该 - 1 
        private Dictionary<string, int> refCountDic = new Dictionary<string, int>();

        //每个被启用的资源，所对应的包名
        private Dictionary<Object, string> resDic = new Dictionary<Object, string>();

        private string PathUrl
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        private string MainABName
        {
            get
            {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
                return "Android";
#elif UNITY_WEBGL
            return "WebGL";
#else
            return "PC";
#endif
            }
        }

        #region 同步加载 - 公开
        //同步加载，不指定类型 (LoadRes 重载1)
        public Object LoadRes(string abName, string resName)
        {
            //判断资源是否已经加载过，如果加载过，直接从引用里面拿即可，不需要重新再生成一份资源
            //加载AB包
            LoadAb(abName);
            Object obj = GetResouInDic(abName, resName);
            //当该资源没有被加载过时候
            if (!obj)
            {
                obj = abDic[abName].LoadAsset(resName);
                resDic.Add(obj, abName);
                //增加引用计数
                AddRefRecord(abName);
            }

            //判断是否GameObject，如果是则直接生成
            return InstantiateObj(obj);
        }


        //同步加载，根据type指定类型(LoadRes 重载2)
        public Object LoadRes(string abName, string resName, System.Type type)
        {
            //加载AB包
            LoadAb(abName);

            Object obj = GetResouInDic(abName, resName);

            //当该资源没有被加载过时候
            if (!obj)
            {
                obj = abDic[abName].LoadAsset(resName, type);
                resDic.Add(obj, abName);
                //增加引用计数
                AddRefRecord(abName);
            }

            //判断是否GameObject，如果是则直接生成
            return InstantiateObj(obj);
        }

        //同步加载，根据泛型指定类型(LoadRes 重载3)
        public T LoadRes<T>(string abName, string resName) where T : Object
        {
            //加载AB包
            LoadAb(abName);

            T obj = (T)GetResouInDic(abName, resName);

            //当该资源没有被加载过时候
            if (!obj)
            {
                obj = abDic[abName].LoadAsset<T>(resName);
                resDic.Add(obj, abName);
                //增加引用计数
                AddRefRecord(abName);
            }

            //判断是否GameObject，如果是则直接生成
            return (T)InstantiateObj(obj);

        }

        #endregion

        #region  异步加载 - 公开
        //这里的异步加载，AB包并没有使用异步加载
        //只是从AB包加载资源时候，使用异步
        //根据名字异步加载
        public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
        {
            //加载AB包
            LoadAsyncAB(abName, () =>
            {
                StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
            });
        }

        private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
        {

            Object obj = GetResouInDic(abName, resName);
            //当该资源没有被加载过时候
            if (!obj)
            {
                AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
                yield return abr;
                obj = abr.asset;
                resDic.Add(obj, abName);
                //增加引用计数
                AddRefRecord(abName);
            }
            //判断是否GameObject，如果是则直接生成
            InstantiateObj(obj);
        }

        //根据Type异步加载资源
        public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
        {
            //加载AB包
            LoadAsyncAB(abName, () => {
                StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
            });
        }

        private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
        {
            Object obj = GetResouInDic(abName, resName);
            //当该资源没有被加载过时候
            if (!obj)
            {
                AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
                yield return abr;
                obj = abr.asset;
                resDic.Add(obj, abName);
                //增加引用计数
                AddRefRecord(abName);
            }
            //判断是否GameObject，如果是则直接生成
            InstantiateObj(obj);
        }

        //根据泛型异步加载资源
        public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
        {
            //加载AB包
            LoadAsyncAB(abName, () =>
            {
                StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
            });
        }

        private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
        {
            Object obj = GetResouInDic(abName, resName);
            //当该资源没有被加载过时候
            if (!obj)
            {
                AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
                yield return abr;
                obj = abr.asset;
                resDic.Add(obj, abName);
                //增加引用计数
                AddRefRecord(abName);
            }
            //判断是否GameObject，如果是则直接生成
            InstantiateObj(obj);
        }
        #endregion

        #region AB包卸载

        /// <summary>
        /// Asset资源卸载
        /// </summary>
        /// <param name="obj"></param>
        public void UnLoadAsset(Object obj, bool isDelete = true)
        {
            //判断是不是包里面的资源
            foreach (var res in resDic)
            {
                if (res.Key == obj)
                {
                    //1.先清理包中的计数(如果计数为0，则卸载AB包)
                    UnLoadAB(resDic[obj], isDelete);
                    //2.清理Res字典
                    resDic.Remove(obj);
                    break;
                }
            }
        }

        //所有包的卸载(慎用) 所有AB包资源都会被干掉,有可能导致资源丢失
        public void ClearAB()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            abDic.Clear();
            dependencies.Clear();
            refCountDic.Clear();
            mainAb = null;
            mainfest = null;
            Resources.UnloadUnusedAssets();
        }

        //根据引用计数，卸载未使用的包
        public void UnLoadUnuseAB()
        {
            List<string> needDelet = new List<string>();
            //遍历当前已经加载的包(获取需要删除的包)
            foreach (var item in abDic)
            {
                if (refCountDic.ContainsKey(item.Key))
                {
                    int ABrefCount = refCountDic.GetValue(item.Key);
                    if (ABrefCount <= 0)
                    {
                        needDelet.Add(item.Key);
                    }
                }
            }

            for (int i = 0; i < needDelet.Count; i++)
            {
                //卸载包
                abDic[needDelet[i]].Unload(false);
                //移除出缓存字典
                abDic.Remove(needDelet[i]);
                //依赖包信息销毁
                dependencies.Remove(needDelet[i]);
            }
        }

        /// <summary>
        /// 单个包卸载, 通常不直接卸载AB包，而是根据资源的使用情况进行卸载
        /// 当某个包没有再使用任何的资源，则应该卸载掉该包
        /// </summary>
        /// <param name="abName">ab包名</param>
        /// <param name="isDelete">如果false,则单单减少引用计数，不卸载包;如果true, 则减少计数的同时检测是否应该卸载包</param>
        public void UnLoadAB(string abName, bool isDelete = true)
        {
            UnLoadInternal(abName, isDelete);
        }

        private void UnLoadInternal(string abName, bool isDelete = true)
        {
            if (abDic.ContainsKey(abName))
            {
                //判断引用计数
                int refNum = RemoveRefRecord(abName);
                if (refNum <= 0)
                {
                    UnLoadDependencies(abName, isDelete);
                    if (isDelete)
                    {
                        abDic[abName].Unload(false);
                        //从缓存中删除
                        abDic.Remove(abName);
                    }
                }
            }
        }

        //卸载依赖包
        private void UnLoadDependencies(string abName, bool isDelete = true)
        {
            string[] dependenciesNes = null;
            //当没有找到引用信息时候，返回
            if (!dependencies.TryGetValue(abName, out dependenciesNes)) return;

            foreach (var dependency in dependenciesNes)
            {
                UnLoadInternal(dependency, isDelete);
            }
            if (isDelete)
            {
                //与其他包没有引用了，移除引用信息
                dependencies.Remove(abName);
            }
        }


        #endregion

        /// <summary>
        ///  加载AB包
        /// </summary>
        /// <param name="abName"></param>
        private void LoadAb(string abName)
        {
            //第一次加载的时候调用
            if (mainAb == null)
            {
                //主要用来获取mainfest列表，即所有AB包的列表
                mainAb = AssetBundle.LoadFromFile(PathUrl + MainABName + "/" + MainABName);
                mainfest = mainAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }

            //获取依赖包相关信息
            AssetBundle ab;
            //从所有包列表里面，找寻指定包的引用，并加载引用
            string[] strs = mainfest.GetAllDependencies(abName);
            //更新依赖包信息
            UpdateDependencies(abName, strs);

            //将所有依赖包标志为加载状态(只要进入abDic字典,则是当前正在加载的包)
            for (int i = 0; i < strs.Length; i++)
            {
                //判断包是否加载过
                if (!abDic.ContainsKey(strs[i]))
                {
                    ab = AssetBundle.LoadFromFile(PathUrl + MainABName + "/" + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //给对应资源增加引用计数
                AddRefRecord(strs[i]);
            }

            //加载资源来源包
            //如果没有加载过，再加载
            if (!abDic.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + MainABName + "/" + abName);
                abDic.Add(abName, ab);
            }
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        private void LoadAsyncAB(string abName, UnityAction callBack)
        {
            StartCoroutine(ReallyLoadAsyncAB(abName, callBack));
        }

        private bool isRealed = false;

        private IEnumerator ReallyLoadAsyncAB(string abName, UnityAction callback)
        {
            //1. 加载AB包
            AssetBundleCreateRequest abcr;

            //如果为Null且没有在加载，则进入加载
            if (mainAb == null && !isRealed)
            {
                isRealed = true;
                //加载所有包列表
                abcr = AssetBundle.LoadFromFileAsync(PathUrl + MainABName + "/" + MainABName);
                yield return abcr;

                mainAb = abcr.assetBundle;
                AssetBundleRequest abr = mainAb.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
                yield return abr;
                mainfest = (AssetBundleManifest)abr.asset;
            }
            //如果有别的文件在加载，则等待
            else
            {
                //每帧都检测是否加载完毕
                while (true)
                {
                    if (mainAb != null && mainfest != null) break;

                    yield return null;
                }
            }

            //2. 获取依赖包相关信息
            string[] strs = mainfest.GetAllDependencies(abName);

            //更新依赖包信息
            UpdateDependencies(abName, strs);

            //从所有包列表里面加载指定包的引用
            for (int i = 0; i < strs.Length; i++)
            {
                //判断包是否加载过
                if (!abDic.ContainsKey(strs[i]))
                {
                    abDic.Add(strs[i], null);
                    abcr = AssetBundle.LoadFromFileAsync(PathUrl + MainABName + "/" + strs[i]);
                    yield return abcr;
                    //真正加载结束后，记录加载完成的AB包
                    abDic[strs[i]] = abcr.assetBundle;
                }
                else
                {
                    //每帧都判断，是否加载好
                    while (!abDic[strs[i]])
                    {
                        yield return null;
                    }
                }

                //给对应资源增加引用计数
                AddRefRecord(strs[i]);
            }

            //3.添加到字典中
            if (!abDic.ContainsKey(abName))
            {
                abDic.Add(abName, null);
                abcr = AssetBundle.LoadFromFileAsync(PathUrl + MainABName + "/" + abName);
                yield return abcr;
                abDic[abName] = abcr.assetBundle;
            }
            else
            {
                while (abDic[abName] == null)
                {
                    yield return null;
                }
            }

            callback();
        }

        //从资源加载缓存池中获取
        public Object GetResouInDic(string abName, string resName)
        {
            Object obj = null;
            foreach (var res in resDic)
            {
                //如果包名匹配，且文件名匹配，则直接获取这个资源
                if (res.Key.name.Equals(resName))
                {
                    if (res.Value.Equals(abName))
                    {
                        obj = res.Key;
                        break;
                    }
                }
            }
            return obj;
        }
        //引用计数增加
        private int AddRefRecord(string abName)
        {
            if (refCountDic.ContainsKey(abName))
            {
                refCountDic[abName]++;
            }
            else
            {
                refCountDic.Add(abName, 1);
            }
            //Debug.Log("当前计数：" + refCountDic[abName]);
            return refCountDic[abName];
        }

        //引用计数减少
        private int RemoveRefRecord(string abName)
        {
            //Debug.Log("当前：" + abName);
            if (refCountDic.ContainsKey(abName))
            {
                //Debug.Log(refCountDic[abName]);
                return --refCountDic[abName];
            }
            return 0;
        }

        //依赖包信息更新
        private void UpdateDependencies(string abName, string[] strs)
        {
            if (!dependencies.ContainsKey(abName))
            {
                dependencies.Add(abName, strs);
            }
            else
            {
                dependencies[abName] = strs;
            }
        }

        //实例化物体
        public Object InstantiateObj(Object obj, UnityAction<Object> callBack = null)
        {
            if (callBack == null)
            {
                if (obj is GameObject)
                    return Instantiate(obj);
                else
                    return obj;
            }
            else
            {
                if (obj is GameObject)
                    callBack(Instantiate(obj));
                else
                    callBack(obj);

                return null;
            }
        }
    }

}
