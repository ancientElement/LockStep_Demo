#define Test
using System;
using System.Globalization;
using AE_ClientNet;
using AE_NetMessage;
using Google.Protobuf.WellKnownTypes;
using NetGameRunning;
using UnityEngine;

namespace LockStep_Demo
{
    public class NetTick : MonoBehaviour
    {
        private int m_curFrame;
        private bool m_reciveFromLastUpLoad;

        private float m_upLoadInterval; //单位秒 间隔多少上传数据
        private float m_timer; //计时器

        PlayerMgr m_playerMgr;

        [SerializeField] private string m_serverIP;
        [SerializeField] private int m_port;
        [SerializeField] private int m_FPS;

        [ContextMenu("开启连接")]
        public void StartConnect()
        {
            NetAsyncMgr.ClearNetMessageListener();

            m_curFrame = -1;
            m_timer = 0;
            SetFPS(m_FPS);
            m_playerMgr = new PlayerMgr();

            NetAsyncMgr.AddNetMessageListener(MessagePool.UpdateMessage_ID, ReciveUpdateMessage);
            NetAsyncMgr.SetMaxMessageFire(m_FPS);

            NetAsyncMgr.Connect(m_serverIP, m_port);
        }

#if Test
        [ContextMenu("测试发送注册自己")]
        public void TestSendRegisterSelfPlayer()
        {
            m_playerMgr.SendRegisterPlayer();
        }

        [ContextMenu("测试开始同步")]
        public void TestSendStartRoom()
        {
            var startRoomMsg = new StartRoomMassage();
            NetAsyncMgr.Send(startRoomMsg);
            AEDebug.Log("开始同步");
        }

        [ContextMenu("测试as需要消耗多少时间")]
        public void TestAsCostTime()
        {
            var oldTime = DateTime.Now;
            for (int i = 0; i < 10000; i++)
            {
                BaseMessage msg = new StartRoomMassage();
                var startRoomMsg = msg as StartRoomMassage;
            }

            var newTime = DateTime.Now;
            var interval = newTime - oldTime;
            AEDebug.Log(interval.TotalMilliseconds);
        }
#endif
        private void Update()
        {
            NetAsyncMgr.FireMessage();
            if (!NetAsyncMgr.IsConnected) return;
            Upload(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!NetAsyncMgr.IsConnected) return;
            m_playerMgr.OnFixedUpdate(Time.fixedDeltaTime);
        }

        /// <summary>
        /// 接收帧数据
        /// </summary>
        /// <param name="msg"></param>
        private void ReciveUpdateMessage(BaseMessage msg)
        {
            var updateMessage = msg as UpdateMessage;
            var updateDate = updateMessage.data;
            if (updateDate.CurFrameIndex == m_curFrame + 1)
            {
                if (updateDate.PlayerInputs[0].JoyX == 0 && updateDate.PlayerInputs[0].JoyY == 0)
                {
                    curZeroInput = true;
                }
                else
                {
                    curZeroInput = false;
                }
                
                m_curFrame = updateDate.CurFrameIndex;
                m_reciveFromLastUpLoad = true;
                m_playerMgr.OnLogincUpdate(updateDate);
            }

            AEDebug.Log(updateDate.Delta);
            AEDebug.Log("接收到第:" + updateDate.CurFrameIndex + "帧数据");


            if (updateDate.PlayerInputs[0].JoyX == 0 && updateDate.PlayerInputs[0].JoyY == 0)
            {
                lastZeroInput = true;
            }
            else
            {
                lastZeroInput = false;
            }
        }

        /// <summary>
        /// 上传玩家消息
        /// </summary>
        private void Upload(float delta)
        {
            //如果没有接收到当前帧则等待
            if (m_playerMgr.PlayerID == -1) return;
            if (!m_reciveFromLastUpLoad) return;
            m_timer += delta;
            if (m_timer >= m_upLoadInterval)
            {
                m_timer = 0;
                UpLoad();
                AEDebug.Log("发布:" + (m_curFrame + 1) + "帧数据");
                m_reciveFromLastUpLoad = false;
            }
        }

        private bool curZeroInput;
        private bool lastZeroInput;

        /// <summary>
        /// 上传玩家消息
        /// </summary>
        private void UpLoad()
        {
            UpLoadMessage upLoadMsg = new UpLoadMessage();
            var playerInput = upLoadMsg.data;

            playerInput.JoyX = Input.GetAxis("Horizontal");
            playerInput.JoyY = Input.GetAxis("Vertical");
            playerInput.PlayerID = m_playerMgr.PlayerID;
            playerInput.CurFrameIndex = m_curFrame + 1;

            NetAsyncMgr.Send(upLoadMsg);
            AEDebug.Log("上传第" + playerInput.CurFrameIndex + "帧的数据" + playerInput.JoyX + "..." + playerInput.JoyY);
        }

        /// <summary>
        /// 设置上传帧率
        /// </summary>
        /// <param name="FPS"></param>
        private void SetFPS(int FPS)
        {
            m_upLoadInterval = 1f / FPS;
        }
    }
}