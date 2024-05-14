using System;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// 接受到消息包事件参数
    /// </summary>
    public class UdpReceiveDataEventArgs : EventArgs
    {
        /// <summary>
        /// 远程地址
        /// </summary>
        private string m_RemoteEndPoint;

        /// <summary>
        /// 数据
        /// </summary>
        private byte[] m_Data;

        /// <summary>
        /// 获取或设置远程地址
        /// </summary>
        public string RemoteEndPoint
        {
            get => this.m_RemoteEndPoint;
            set => this.m_RemoteEndPoint = value;
        }

        /// <summary>
        /// 获取或设置数据
        /// </summary>
        public byte[] Data
        {
            get => this.m_Data;
            set => this.m_Data = value;
        }
    }
}