using System;

namespace SimpleFramework.Udp
{
    /// <summary>
    /// UDP启动事件参数
    /// </summary>
    public class UdpLaunchEventArgs : EventArgs
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_ErrorMessage;

        /// <summary>
        /// 获取或设置错误信息
        /// </summary>
        public string ErrorMessage
        {
            get => this.m_ErrorMessage;
            set => this.m_ErrorMessage = value;
        }
    }
}