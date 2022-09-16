/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-29 
 *说明:    
**/
using MyDemo;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyDemo
{
    public static class GameDataSaveManager_Demo
    {
        /// <summary>
        /// 保存为Json文件
        /// </summary>
        /// <param name="playerData_Json"></param>
        public static void SaveData(GameDataSave_Demo playerData_Json)
        {
            string json = JsonMapper.ToJson(playerData_Json);

            WriteStringToFile(GetPath("GameDataSave_Demo"), json);
        }

        /// <summary>
        /// 读取Json文件中的数据
        /// </summary>
        /// <returns></returns>
        public static GameDataSave_Demo LoadData()
        {
            string JsonString = ReadFileToString(GetPath("GameDataSave_Demo"));

            if (JsonString.IsNotNull())
            {
                return JsonMapper.ToObject<GameDataSave_Demo>(JsonString);
            }
            return null;
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        static string GetPath(string FileName)
        {
#if UNITY_EDITOR
            string filepath = Application.dataPath + "/GameExample/10.经营类游戏地块解锁方案_Demo/" + FileName + ".txt";
#elif UNITY_IPHONE
        //没有试过
        string filepath = Application.dataPath + "/Raw/" + FileName  + ".txt";
#elif UNITY_ANDROID
        //android路径： /storage/emulated/0/Android/data/包名/files/FileName.txt
        string filepath = Application.persistentDataPath + "/" + FileName  + ".txt";
#endif
            return filepath;
        }

        /// <summary>
        /// 写入数据到本地(IO操作)
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static bool WriteStringToFile(string fullPath, string data)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                File.WriteAllText(fullPath, data);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("WriteStringToFile fail, file=[{0}], Exception msg:[{1}]", fullPath, ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// 从本地读取文件数据(IO操作)
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        static string ReadFileToString(string fullPath)
        {
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception)
            {
                //Debug.LogError(string.Format("FileToString fail, file=[{0}], Exception msg:[{1}]", fullPath, ex.ToString()));
                return null;
            }
        }

    }

}
