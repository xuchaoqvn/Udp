using System;
using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// Udp启动处理类
    /// </summary>
    public abstract class UdpLaunchHandlerBase : ScriptableObject
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        /// <param name="message">消息</param>
        public abstract void Handler(UdpComponent udpComponent, string message);
    }
}