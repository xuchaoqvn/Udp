using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// 串口接受的数据处理基类
    /// </summary>
    [CreateAssetMenu(fileName = "UdpReceiveDataHandler", menuName = "Udp/DefaultReceiveData", order = 2)]
    public class DefaultUdpReceiveDataHandler : UdpReceiveDataHandlerBase
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="udpComponent">Udp组件</param>
        /// <param name="data">接受到的数据</param>
        public override void Handler(UdpComponent udpComponent, byte[] data)
        {
            Debug.Log($"Udp Receive Data, Byte Array Length: {data.Length}");
        }
    }
}
