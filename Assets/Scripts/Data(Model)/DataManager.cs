/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    数据少可直接放此处，数据多时需要进行分类, 此处为数据存储区，
 * 即便是从文件读出来的数据，也应当放在此处
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DataManager : Sigleton<DataManager>
{
    //玩家信息(如果没有可删除)
    //PlayerInfor m_PlayerInfor;

    //关卡的信息(如果没有可删除)
    //ScenceInfor m_ScenceInfor;

    //游戏帧数
    public int targetFrameRate = 60;

    //游戏的结果
    public bool gameResult;

    //初始化
    public void Init()
    {
        //初始化持久化数据存储路径
        //InitPath();

    }

    #region 持久化数据存储

    //玩家信息存储目录
    private string playerData_filepath;

    //初始化路径
    private void InitPath()
    {
        //获取玩家存储路径
#if UNITY_EDITOR
        playerData_filepath = Application.dataPath + "/Scripts/Data/" + "PlayerData" + ".txt";
#elif UNITY_IPHONE
        //没有试过
        playerData_filepath = Application.dataPath + "/Raw/" + "PlayerData" + ".txt";
#elif UNITY_ANDROID
        //android路径： /storage/emulated/0/Android/data/包名/files/PlayerData.txt
        playerData_filepath = Application.persistentDataPath + "/" + "PlayerData" + ".txt";
#endif
    }

    //-------------以下需要根据项目的情况进行修改----------------
    //存储玩家数据
    private void Save2Json_PlayerData(string path, PlayerData data)
    {
        //string str = JsonConvert.SerializeObject(data);
        //WriteStringToFile(path, str);
    }

    //读取玩家数据
    private void LoadJson_PlayerData(string path)
    {
        //用Try判断是否能读取数据，如果不能读取，则生成一份数据存进去
        //try
        //{
        //    //判断是否存在该文件，如果存在则直接读取。不存在则重新创建
        //    if (!File.Exists(path))
        //    {
        //        playerData = new PlayerData();
        //        Save2Json_PlayerData(path, playerData);
        //    }
        //    string str = File.ReadAllText(path);
        //    playerData = JsonConvert.DeserializeObject<PlayerData>(str);
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
    }

    //写入string数据到本地
    private bool WriteStringToFile(string fullPath, string data)
    {
        //try
        //{
        //    string directoryName = Path.GetDirectoryName(fullPath);
        //    if (!Directory.Exists(directoryName))
        //    {
        //        Directory.CreateDirectory(directoryName);
        //    }
        //    File.WriteAllText(fullPath, data);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError(string.Format("WriteStringToFile fail, file=[{0}], Exception msg:[{1}]", fullPath, ex.ToString()));
        //    return false;
        //}

        return false;
    }

    #endregion
}
