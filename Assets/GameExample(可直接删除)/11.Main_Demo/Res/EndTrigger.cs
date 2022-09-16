/** 
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-08-04 
 *说明:    //终点的触发器
**/  

using UnityEngine;  
using System.Collections;

namespace MyDemo
{
    public class EndTrigger : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(TagType.Player))
            {
                Messenger.Broadcast(EventType.GameWin);
            }
        }

    }
}
