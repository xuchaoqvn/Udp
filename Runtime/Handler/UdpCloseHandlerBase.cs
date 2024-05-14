using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// Udp错误处理类
    /// </summary>
    public abstract class UdpCloseHandlerBase : ScriptableObject
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        public abstract void Handler(UdpComponent udpComponent);
    }
}