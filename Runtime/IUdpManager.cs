using System;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// Udp管理器接口
    /// </summary>
    public interface IUdpManager : IDisposable
    {
        #region Event
        /// <summary>
        /// 当启动时
        /// </summary>
        event EventHandler Launch;

        /// <summary>
        /// 当客户端错误时
        /// </summary>
        event EventHandler Error;

        /// <summary>
        /// 当关闭时
        /// </summary>
        event EventHandler Close;

        /// <summary>
        /// 当接受到数据包时
        /// </summary>
        event EventHandler ReceiveData;
        #endregion

        #region Property
        /// <summary>
        /// 获取IP
        /// </summary>
        string IP
        {
            get;
        }

        /// <summary>
        /// 获取端口
        /// </summary>
        int Port
        {
            get;
        }

        /// <summary>
        /// 获取是否运行
        /// </summary>
        bool IsRun
        {
            get;
        }

        /// <summary>
        /// 获取发送的数据次数
        /// </summary>
        int SendDataCount
        {
            get;
        }

        /// <summary>
        /// 获取接受的数据次数
        /// </summary>
        int ReceivedDataCount
        {
            get;
        }
        #endregion

        #region Function
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="localIP">本地IP</param>
        /// <param name="localPort">本地端口</param>
        void LaunchUdp(string localIP, int localPort);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ip">远程IP</param>
        /// <param name="port">远程端口</param>
        /// <param name="data">待发送数据</param>
        void Send(string ip, int port, byte[] data);

        /// <summary>
        /// 关闭Udp
        /// </summary>
        void CloseUdp();
        #endregion
    }
}