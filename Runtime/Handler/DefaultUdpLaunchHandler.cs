
using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// 打开串口和关闭串口处理基类
    /// </summary>
    [CreateAssetMenu(fileName = "UdpLaunchHandler", menuName = "Udp/DefaultLaunch", order = 0)]
    public class DefaultUdpLaunchHandler : UdpLaunchHandlerBase
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        /// <param name="message">消息</param>
        public override void Handler(UdpComponent udpComponent, string message)
        {
            if (string.IsNullOrEmpty(message))
                Debug.Log("Udp Launch.");
            else
                Debug.LogError($"Udp Launch Failure,{message}");
        }
    }
}