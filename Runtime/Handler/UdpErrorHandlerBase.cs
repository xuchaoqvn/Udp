using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// 串口错误处理类
    /// </summary>
    public abstract class UdpErrorHandlerBase : ScriptableObject
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        /// <param name="errorMessage">错误消息</param>
        public abstract void Handler(UdpComponent udpComponent, string errorMessage);
    }
}