using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// 串口错误处理基类
    /// </summary>
    [CreateAssetMenu(fileName = "UdpErrorHandler", menuName = "Udp/DefaultError", order = 1)]
    public class DefaultUdpErrorHandler : UdpErrorHandlerBase
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        /// <param name="errorMessage">错误消息</param>
        public override void Handler(UdpComponent udpComponent, string errorMessage)
        {
            Debug.LogError(errorMessage);
        }
    }
}