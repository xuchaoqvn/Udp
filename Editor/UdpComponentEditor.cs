using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleFramework.Udp.Editor
{
    /// <summary>
    /// Udp组件Inspector面板类
    /// </summary>
    [CustomEditor(typeof(UdpComponent), true)]
    [CanEditMultipleObjects]
    public class UdpComponentEditor : UnityEditor.Editor
    {
        #region Field
        /// <summary>
        /// Udp组件
        /// </summary>
        private UdpComponent m_UdpComponent;

        /// <summary>
        /// IP
        /// </summary>
        private SerializedProperty m_IP;

        /// <summary>
        /// 端口
        /// </summary>
        private SerializedProperty m_Port;

        /// <summary>
        /// Udp启动处理类
        /// </summary>
        private SerializedProperty m_UdpLaunchHandler;

        /// <summary>
        /// Udp接受数据处理类
        /// </summary>
        private SerializedProperty m_UdpErrorHandler;

        /// <summary>
        /// Udp错误处理类
        /// </summary>
        private SerializedProperty m_UdpReceiveDataHandler;

        /// <summary>
        /// Udp错误处理类
        /// </summary>
        private SerializedProperty m_UdpCloseHandler;
        #endregion

        #region Function
        protected virtual void OnEnable()
        {
            this.m_UdpComponent = this.serializedObject.targetObject as UdpComponent;

            this.m_IP = this.serializedObject.FindProperty("m_IP");
            this.m_Port = this.serializedObject.FindProperty("m_Port");
            this.m_UdpLaunchHandler = this.serializedObject.FindProperty("m_UdpLaunchHandler");
            this.m_UdpErrorHandler = this.serializedObject.FindProperty("m_UdpErrorHandler");
            this.m_UdpReceiveDataHandler = this.serializedObject.FindProperty("m_UdpReceiveDataHandler");
            this.m_UdpCloseHandler = this.serializedObject.FindProperty("m_UdpCloseHandler");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            if (EditorApplication.isPlaying)
            {
                if (!this.m_UdpComponent.IsRun)
                    this.DrawNoOpenGUI();
                else
                    this.DrawOpenGUI();
            }
            else
                this.DrawNoOpenGUI();

            this.serializedObject.ApplyModifiedProperties();
        }

        private void DrawOpenGUI()
        {
            EditorGUILayout.LabelField($"IP：{this.m_UdpComponent.IP}");
            EditorGUILayout.LabelField($"端口：{this.m_UdpComponent.Port}");
            EditorGUILayout.LabelField($"发送的数据次数：{this.m_UdpComponent.SendDataCount}");
            EditorGUILayout.LabelField($"接收的数据次数：{this.m_UdpComponent.ReceivedDataCount}");
        }

        private void DrawNoOpenGUI()
        {
            EditorGUILayout.PropertyField(this.m_IP, new GUIContent("IP"));
            EditorGUILayout.PropertyField(this.m_Port, new GUIContent("端口"));
            EditorGUILayout.PropertyField(this.m_UdpLaunchHandler, new GUIContent("打开Udp处理类"));
            EditorGUILayout.PropertyField(this.m_UdpErrorHandler, new GUIContent("Udp错误处理类"));
            EditorGUILayout.PropertyField(this.m_UdpReceiveDataHandler, new GUIContent("Udp接受数据处理类"));
            EditorGUILayout.PropertyField(this.m_UdpCloseHandler, new GUIContent("Udp关闭处理类"));

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(EditorGUIUtility.labelWidth);
                if (GUILayout.Button("设置默认IP和端口", EditorStyles.miniButton))
                    this.m_UdpComponent.SetDefaultIpAndPort();
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}