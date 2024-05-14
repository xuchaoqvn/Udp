using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// Udp接受的数据处理类
    /// </summary>
    public abstract class UdpReceiveDataHandlerBase : ScriptableObject
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        /// <param name="data">接受到的数据</param>
        public abstract void Handler(UdpComponent udpComponent, byte[] data);
    }
}