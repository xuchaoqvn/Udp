
using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// 打开串口和关闭串口处理基类
    /// </summary>
    [CreateAssetMenu(fileName = "UdpCloseHandler", menuName = "Udp/DefaultClose", order = 3)]
    public class DefaultUdpCloseHandler : UdpCloseHandlerBase
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        public override void Handler(UdpComponent udpComponent)
        {
            Debug.Log("Udp Closed.");
        }
    }
}