using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// Udp组件
    /// </summary>
    public class UdpComponent : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// Ud管理器
        /// </summary>
        private IUdpManager m_UdpManager;

        /// <summary>
        /// ip
        /// </summary>
        [SerializeField]
        private string m_IP;

        /// <summary>
        /// 端口
        /// </summary>
        [SerializeField]
        private int m_Port = 8306;

        /// <summary>
        /// 打开Udp处理类
        /// </summary>
        [SerializeField]
        private UdpLaunchHandlerBase m_UdpLaunchHandler;

        /// <summary>
        /// Udp错误处理类
        /// </summary>
        [SerializeField]
        private UdpErrorHandlerBase m_UdpErrorHandler;

        /// <summary>
        /// Udp接受数据处理类
        /// </summary>
        [SerializeField]
        private UdpReceiveDataHandlerBase m_UdpReceiveDataHandler;

        /// <summary>
        /// Udp关闭处理类
        /// </summary>
        [SerializeField]
        private UdpCloseHandlerBase m_UdpCloseHandler;

        /// <summary>
        /// 消息队列
        /// </summary>
        private Queue<EventArgs> m_ReceiveData;
        #endregion

        #region Property
        /// <summary>
        /// 获取IP
        /// </summary>
        public string IP => this.m_UdpManager.IP;

        /// <summary>
        /// 获取端口
        /// </summary>
        public int Port => this.m_UdpManager.Port;

        /// <summary>
        /// 获取是否运行
        /// </summary>
        public bool IsRun => this.m_UdpManager.IsRun;

        /// <summary>
        /// 获取发送的数据次数
        /// </summary>
        public int SendDataCount => this.m_UdpManager.SendDataCount;

        /// <summary>
        /// 获取接受的数据次数
        /// </summary>
        public int ReceivedDataCount => this.m_UdpManager.ReceivedDataCount;
        #endregion

        private void OnEnable()
        {
            this.m_UdpManager = new UdpManager();
            this.m_UdpManager.Launch += this.OnUdpLaunch;
            this.m_UdpManager.Error += this.OnUdpError;
            this.m_UdpManager.ReceiveData += this.OnUdpReceiveData;
            this.m_UdpManager.Close += this.OnUdpClose;
        }

        private void Update()
        {
            if (this.m_ReceiveData == null)
                return;

            while (this.m_ReceiveData.Count > 0)
            {
                EventArgs eventArgs = this.m_ReceiveData.Dequeue();
                if (eventArgs is UdpLaunchEventArgs)
                {
                    UdpLaunchEventArgs udpLaunchEventArgs = eventArgs as UdpLaunchEventArgs;
                    this.m_UdpLaunchHandler.Handler(this, udpLaunchEventArgs.ErrorMessage);
                }
                if (eventArgs is UdpErrorEventArgs)
                {
                    UdpErrorEventArgs udpErrorEventArgs = eventArgs as UdpErrorEventArgs;
                    this.m_UdpErrorHandler.Handler(this, udpErrorEventArgs.ErrorMessage);
                }
                else if (eventArgs is UdpReceiveDataEventArgs)
                {
                    UdpReceiveDataEventArgs udpReceiveDataEventArgs = eventArgs as UdpReceiveDataEventArgs;
                    this.m_UdpReceiveDataHandler.Handler(this, udpReceiveDataEventArgs.Data);
                }
                else if (eventArgs is UdpCloseEventArgs)
                {
                    UdpCloseEventArgs udpCloseEventArgs = eventArgs as UdpCloseEventArgs;
                    this.m_UdpCloseHandler.Handler(this);
                }
            }
        }

        private void OnDisable()
        {
            if (!this.m_UdpManager.IsRun)
            {
                this.m_UdpManager.Launch -= this.OnUdpLaunch;
                this.m_UdpManager.Error -= this.OnUdpError;
                this.m_UdpManager.ReceiveData -= this.OnUdpReceiveData;
                this.m_UdpManager.Close -= this.OnUdpClose;
                this.m_UdpManager.Dispose();
                this.m_UdpManager = null;
            }
        }

        #region Function
        /// <summary>
        /// 启动
        /// </summary>
        public void LaunchUdp() => this.m_UdpManager.LaunchUdp(this.m_IP, this.m_Port);

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        public void LaunchUdp(string ip, int port)
        {
            this.m_IP = ip;
            this.m_Port = port;

            this.m_UdpManager.LaunchUdp(this.m_IP, this.m_Port);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ip">远程IP</param>
        /// <param name="port">远程端口</param>
        /// <param name="data">待发送数据</param>
        public void Send(string ip, int port, byte[] data) => this.m_UdpManager.Send(ip, port, data);

        /// <summary>
        /// 关闭Udp
        /// </summary>
        public void CloseUdp() => this.m_UdpManager.CloseUdp();

        /// <summary>
        /// 当Udp启动时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUdpLaunch(object sender, EventArgs e)
        {
            this.m_ReceiveData = new Queue<EventArgs>();
            this.m_ReceiveData.Enqueue(e);
        }

        /// <summary>
        /// 当Udp发生错误时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUdpError(object sender, EventArgs e) => this.m_ReceiveData.Enqueue(e);

        /// <summary>
        /// 当Udp接受到数据时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUdpReceiveData(object sender, EventArgs e) => this.m_ReceiveData.Enqueue(e);

        /// <summary>
        /// 当Udp关闭时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUdpClose(object sender, EventArgs e) => this.m_ReceiveData.Enqueue(e);

        /// <summary>
        /// 设置默认IP和端口
        /// </summary>
        public void SetDefaultIpAndPort()
        {
            this.m_IP = this.GetLocalIp();
            this.m_Port = 8306;
        }

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns>本地IP</returns>
        private string GetLocalIp()
        {
            string ip = "127.0.0.1";
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = IpEntry.AddressList[i].ToString();
                        return ip;
                    }
                }
                return ip;
            }
            catch (Exception)
            {
                return ip;
            }
        }
        #endregion
    }
}