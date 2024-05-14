using System;
using System.Net;
using System.Net.Sockets;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// UDP
    /// </summary>
    internal class UdpManager : IUdpManager
    {
        #region EventHandler
        /// <summary>
        /// 当启动时
        /// </summary>
        private EventHandler m_OnLaunch;

        /// <summary>
        /// 当客户端错误时
        /// </summary>
        private EventHandler m_OnError;

        /// <summary>
        /// 当关闭连接时
        /// </summary>
        private EventHandler m_OnClose;

        /// <summary>
        /// 当接受到数据时
        /// </summary>
        private EventHandler m_OnReceiveData;
        #endregion

        #region Field
        /// <summary>
        /// ip
        /// </summary>
        private string m_IP;

        /// <summary>
        /// 端口
        /// </summary>
        private int m_Port;

        /// <summary>
        /// 套接字
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// 缓冲数组
        /// </summary>
        private byte[] m_Data;

        /// <summary>
        /// 是否运行
        /// </summary>
        private bool m_IsRun;

        /// <summary>
        /// 发送的数据数量
        /// </summary>
        private int m_SendDataCount;

        /// <summary>
        /// 接受到的数据数量
        /// </summary>
        private int m_ReceivedDataCount;

        /// <summary>
        /// 远程地址
        /// </summary>
        private EndPoint m_RemoteEndPoint;
        #endregion

        #region Property
        /// <summary>
        /// 获取IP
        /// </summary>
        public string IP => this.m_IP;

        /// <summary>
        /// 获取端口
        /// </summary>
        public int Port => this.m_Port;

        /// <summary>
        /// 获取是否运行
        /// </summary>
        public bool IsRun => this.m_IsRun;

        /// <summary>
        /// 获取发送的数据次数
        /// </summary>
        public int SendDataCount => this.m_SendDataCount;

        /// <summary>
        /// 获取接受的数据次数
        /// </summary>
        public int ReceivedDataCount => this.m_ReceivedDataCount;
        #endregion

        #region Event
        /// <summary>
        /// 当启动时
        /// </summary>
        public event EventHandler Launch
        {
            add { this.m_OnLaunch += value; }
            remove { this.m_OnLaunch -= value; }
        }

        /// <summary>
        /// 当客户端错误时
        /// </summary>
        public event EventHandler Error
        {
            add { this.m_OnError += value; }
            remove { this.m_OnError -= value; }
        }

        /// <summary>
        /// 当关闭时
        /// </summary>
        public event EventHandler Close
        {
            add { this.m_OnClose += value; }
            remove { this.m_OnClose -= value; }
        }

        /// <summary>
        /// 当接受到数据包时
        /// </summary>
        public event EventHandler ReceiveData
        {
            add { this.m_OnReceiveData += value; }
            remove { this.m_OnReceiveData -= value; }
        }
        #endregion

        public UdpManager()
        {
            this.m_RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        #region Function
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        public void LaunchUdp(string ip, int port)
        {
            if (this.m_IsRun || this.m_Socket != null)
            {
                UdpLaunchEventArgs udpLaunchEventArgs = new UdpLaunchEventArgs();
                udpLaunchEventArgs.ErrorMessage = "Udp Already Launch...";
                this.m_OnLaunch?.Invoke(this, udpLaunchEventArgs);
                return;
            }

            try
            {
                this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this.m_Socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            }
            catch (Exception exception)
            {
                this.m_OnLaunch?.Invoke(this, new UdpLaunchEventArgs() { ErrorMessage = exception.ToString() });
                return;
            }

            this.m_Data = new byte[1024 * 64];
            this.m_IP = ip;
            this.m_Port = port;
            this.m_IsRun = true;
            this.m_OnLaunch?.Invoke(this, new UdpLaunchEventArgs());
            this.BeginReceive();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ip">远程IP</param>
        /// <param name="port">远程端口</param>
        /// <param name="data">待发送数据</param>
        public void Send(string ip, int port, byte[] data)
        {
            this.BeginSend(ip, port, data);
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="ip">远程IP</param>
        /// <param name="port">远程端口</param>
        /// <param name="data">待发送数据</param>
        private void BeginSend(string ip, int port, byte[] data)
        {
            if (!this.m_IsRun)
                return;

            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                this.m_Socket.BeginSendTo(data, 0, data.Length, SocketFlags.None, iPEndPoint, this.EndSend, this.m_Socket);
            }
            catch (Exception exception)
            {
                this.m_OnError?.Invoke(this, new UdpErrorEventArgs() { ErrorMessage = exception.ToString() });
            }
        }

        /// <summary>
        /// 异步结束发送数据
        /// </summary>
        /// <param name="ar"></param>
        private void EndSend(IAsyncResult ar)
        {
            if (!this.m_IsRun)
                return;

            Socket socket = ar.AsyncState as Socket;
            int sendDataLength = 0;
            try
            {
                sendDataLength = socket.EndSendTo(ar);
                this.m_SendDataCount++;
            }
            catch (Exception exception)
            {
                this.m_OnError?.Invoke(this, new UdpErrorEventArgs() { ErrorMessage = exception.ToString() });
            }
        }

        /// <summary>
        /// 异步开始接收数据
        /// </summary>
        private void BeginReceive()
        {
            if (!this.m_IsRun)
                return;
            try
            {
                this.m_Socket.BeginReceiveFrom(this.m_Data, 0, this.m_Data.Length, SocketFlags.None, ref this.m_RemoteEndPoint, this.EndReceive, this.m_Socket);
            }
            catch (Exception exception)
            {
                this.m_OnError?.Invoke(this, new UdpErrorEventArgs() { ErrorMessage = exception.ToString() });
            }
        }

        /// <summary>
        /// 异步结束接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void EndReceive(IAsyncResult ar)
        {
            if (!this.m_IsRun)
                return;

            Socket socket = ar.AsyncState as Socket;
            int receiveDataLength = 0;
            try
            {
                receiveDataLength = socket.EndReceiveFrom(ar, ref this.m_RemoteEndPoint);
            }
            catch (Exception exception)
            {
                this.m_OnError?.Invoke(this, new UdpErrorEventArgs() { ErrorMessage = exception.ToString() });
            }

            if (receiveDataLength <= 0)
            {
                this.BeginReceive();
                return;
            }

            byte[] data = new byte[receiveDataLength];
            Buffer.BlockCopy(this.m_Data, 0, data, 0, receiveDataLength);
            this.m_OnReceiveData?.Invoke(this, new UdpReceiveDataEventArgs() { RemoteEndPoint = this.m_RemoteEndPoint.ToString(), Data = data });
            this.m_ReceivedDataCount++;

            this.BeginReceive();
        }

        /// <summary>
        /// 关闭Udp
        /// </summary>
        public void CloseUdp()
        {
            try
            {
                this.m_Socket?.Close();
            }
            catch (Exception exception)
            {
                UdpErrorEventArgs udpErrorEventArgs = new UdpErrorEventArgs();
                udpErrorEventArgs.ErrorMessage = exception.ToString();
                this.m_OnError?.Invoke(this, udpErrorEventArgs);
                return;
            }

            this.m_IsRun = false;
            this.m_OnClose?.Invoke(this, new UdpCloseEventArgs());
        }
        #endregion

        #region Dispose
        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool m_Disposed;

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.m_Disposed)
            {
                if (disposing)
                {
                    //释放托管状态(托管对象)
                    if (this.m_Data != null)
                    {
                        Array.Clear(this.m_Data, 0, this.m_Data.Length);
                        this.m_Data = null;
                    }
                }

                // 释放未托管的资源(未托管的对象)并替代终结器
                // 将大型字段设置为 null
                //此处回调
                try
                {
                    this.m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {

                }
                finally
                {
                    this.m_Socket?.Close();
                    this.m_Socket = null;
                }

                this.m_IsRun = false;
                this.m_Disposed = true;
                this.m_OnClose?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        // 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~UdpManager()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
