/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-21 
 *说明:    游戏世界管理器， 应该管理世界场景中的各种游戏物体，比如玩家，比如怪物
 *         游戏主管理器(GameMain)可以从中访问到个体, 并且传递消息
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDemo
{
    public class GameWorldManager : MonoBehaviour
    {
        Player m_player;                                                                //玩家

        ScenceData m_scenceInfo;                                                        //场景信息

        public List<GameObject> ScenceList;                                             //做好的关卡场景

        public GameObject playerObj;                                                    //做好的人物模型

        //加载场景中的物体(初始化的操作应该放于GameMain)
        public void GameWorld_Init(int num)
        {
            //1.生成一个新的场景(需要动态加载时启用)
            LoadLevel(num);
            //2.在场景中创建玩家(需要动态加载时启用)
            CreatPlayer("Player", GameManager.Instance.GetPlayerInitPos());
        }

        #region 场景的初始化方式
        public void LoadLevel(int num)
        {
            //检测当前场景是否已经存在，有则删除(此处如果场景过大，可以进行一个预加载操作)
            DestroyGameScence();
            //当输入的关卡值，超出了已有关卡数的范围。要进行调整。否则保持原来的值
            //if (num > GameManager.Instance.m_DataManger.LevelNum)
            //{
            //    //由于关卡需要反复游玩, 所以需要对num进行判断操作(获取当前实际的关卡数量进行计算，应该加载哪张地图)
            //    num = num % GameManager.Instance.m_DataManger.LevelNum;
            //    //如果num求余为0，则当前关卡为最后一关，否则当前关卡为其余数
            //    num = num == 0 ? GameManager.Instance.m_DataManger.LevelNum : num;
            //}
            if (num > ScenceList.Count)
            {
                //由于关卡需要反复游玩, 所以需要对num进行判断操作(获取当前实际的关卡数量进行计算，应该加载哪张地图)
                num = num % ScenceList.Count;
                //如果num求余为0，则当前关卡为最后一关，否则当前关卡为其余数
                num = num == 0 ? ScenceList.Count : num;
            }

            //一方案： 动态加载游戏场景
            //string path = string.Format("Level/Level_{0}", num);
            //GameObject CurrentScence = Instantiate(Resources.Load(path)) as GameObject;
            //二方案:  挂个公有类 List<GameObject> 在上面，按序提取... 由于是Demo，所以简单粗暴直接读取
            GameObject CurrentScence = Instantiate(ScenceList[num - 1]) as GameObject;
            //重新设置父类(放在GameWorld下面)
            CurrentScence.transform.SetParent(transform, false);
            //获取新生成的场景上挂的"场景信息"脚本
            m_scenceInfo = CurrentScence.GetComponent<ScenceData>();
            //重新设置名字
            CurrentScence.name = "Level_" + num.ToString();
        }

        //在场景中生成玩家 参数:玩家的名字， 玩家的位置
        public void CreatPlayer(string playerName, Vector3 playerVec3)
        {
            //检测当前场景中有没有玩家角色，有则删除了再生成一个
            DestroyPlayer();
            //一方案：获取玩家预制体的目录
            //string path = string.Format("Character/{0}", playerName);
            //动态加载玩家个体
            //m_player = (Instantiate(Resources.Load(path)) as GameObject).GetComponent<Player>();
            //二方案：
            m_player = (Instantiate(playerObj)).GetComponent<Player>();
            //重新设置父类
            m_player.transform.SetParent(transform, false);
            //根据场景Index初始化玩家的位置
            m_player.transform.position = playerVec3;
            //重新设置名字
            m_player.name = playerName;
        }

        //删除玩家角色(当玩家角色不为空的时候)
        public void DestroyPlayer()
        {
            if (m_player != null)
            {
                Destroy(m_player.gameObject);
            }
        }

        //摧毁当前场景(如果有)
        public void DestroyGameScence()
        {
            if (m_scenceInfo != null)
            {
                Destroy(m_scenceInfo.gameObject);
            }
        }

        //获取当前关卡
        public int GetScenceIndex()
        {
            return m_scenceInfo.ScenceIndex;
        }

        #endregion

        #region 玩家的操作方法
        //获取玩家目标
        public Transform GetPlayer()
        {
            if (m_player != null)
            {
                return m_player.transform;
            }
            return null;
        }

        //设置玩家是否能够移动
        public void SetPlayerMove(bool canMove)
        {
            if (m_player != null)
            {
                m_player.CanMove = canMove;
            }
        }

        //设置玩家的移动速度
        public void SetSpeedMul_Player(float num)
        {
            if (m_player != null)
            {
                m_player.SetSpeedMul_Player(num);
            }
        }

        //改变玩家的状态
        public void ChangePlayerState(PlayerState state)
        {
            if (m_player != null)
            {
                m_player.ChangeState(state);
            }
        }

        //通知玩家播放某个动作
        public void PlayAnimation_Player(string triggleName)
        {
            m_player.PlayAnimation(triggleName);
        }

        //给玩家发送信息
        public void HandleMessage_Player(MessageType playerMessage, params object[] parm)
        {
            m_player.HandleMessage(playerMessage, parm);
        }
        #endregion
    }


}
