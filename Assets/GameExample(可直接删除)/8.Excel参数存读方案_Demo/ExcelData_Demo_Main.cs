/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-11-26 
 *说明:    
**/  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.IO;

namespace MyDemo
{
    /// <summary>
    /// 实际上就是  Excel 转 -->  Json  转 --> 数据类 
    /// 但Json不好观察， Excel不好读取，所以需用间接的方法，读取到游戏中
    /// </summary>
    public class ExcelData_Demo_Main : MonoBehaviour
    {
        //此处为了方便，直接定义public拖拽获取；
        //也可以通过读本地文件的方式获取 （Resouce, AB包加载等）
        public TextAsset WeaponsTypeOfJson;

        public Weapons[] weapons;

        private void Start()
        {
            //-----------------------------------------参数表读取----------------------------- (读取策划配置的参数)
            ParseWeaponsTypeJson(WeaponsTypeOfJson);

            for (int i = 0; i < weapons.Length; i++)
            {
                Debug.Log("当前武器名字：" + weapons[i].Name + "   1级武器造价: " + weapons[i].Level_1 + "   2级武器造价: " + weapons[i].Level_2);
            }

            //-------------------------------------Json存储方案-------------------------------------
            //通常不喜欢用Json进行存储数据，因为PlayerPrefabs更方便，如不需要这段可忽略
            //已内置 GameSaveManager.cs 进行数据存储
            PlayerData_Json playerData = new PlayerData_Json() { Name = "Jack", age = 11, sex = "male" };

            SavePlayerData_Json(playerData, "PlayerData_Json");
        }

        //解析Json文件
        private void ParseWeaponsTypeJson(TextAsset JsonText)
        {
            //有时候LitJson会在某些数据转换不过来而报错，比如 Double -> int, 则需要添加此代码。其他类型也如此
            JsonMapper.RegisterImporter<double, int>((double input) => { return Convert.ToInt32(input); });
            weapons = JsonMapper.ToObject<Weapons[]>(JsonText.text);
        }

        //保存为Json文件(单数据)
        private void SavePlayerData_Json(PlayerData_Json playerData_Json, string FileName)
        {
            string json = JsonMapper.ToJson(playerData_Json);

            //获取存储路径
#if UNITY_EDITOR
            string filepath = Application.dataPath + "/GameExample/8.Excel数据存读方案_Demo/" + FileName + ".txt";
#elif UNITY_IPHONE
        //没有试过
        string filepath = Application.dataPath + "/Raw/" + FileName + ".txt";
#elif UNITY_ANDROID
        //android路径： /storage/emulated/0/Android/data/包名/files/FileName.txt
        string filepath = Application.persistentDataPath + "/" + FileName + ".txt";
#endif
            WriteStringToFile(filepath, json);
        }

        //写入数据到本地
        private bool WriteStringToFile(string fullPath, string data)
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

        //写入JsonData到本地
        private bool WriteJsonToFile(string fullPath, JsonData data)
        {
            return WriteStringToFile(fullPath, data.ToJson());
        }
    }
}

