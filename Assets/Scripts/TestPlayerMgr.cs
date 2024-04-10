using System;
using AE_BEPUPhysics_Addition;
using NetGameRunning;
using UnityEngine;

namespace LockStep_Demo.Assets.Scripts
{
    public class TestPlayerMgr : MonoBehaviour
    {
        private PlayerMgr m_PlayerMgr;
        [SerializeField] private int m_playerCount;
        [SerializeField] private bool m_startedSync;
        private float m_upLoadInterval; //单位秒 间隔多少上传数据
        [SerializeField] private int m_FPS;
        private float m_timer;
        private AEPhysicsMgr m_AEPhysicsMgr;


        private void Awake()
        {
            m_AEPhysicsMgr = new AEPhysicsMgr(new BEPUutilities.Vector3(0, -9.8m, 0));
            m_PlayerMgr = new PlayerMgr(m_AEPhysicsMgr);
            m_upLoadInterval = 1f / m_FPS;  
        }

        [ContextMenu("测试注册玩家")]
        public void TestRegisterPlayer()
        {
            RegisterMessage registerMsg = new RegisterMessage();
            registerMsg.data.PlayerID = m_playerCount;
            m_playerCount += 1;
            m_PlayerMgr.ReciveRegisterPlayer(registerMsg);
        }

        [ContextMenu("测试注册自己")]
        public void TestRegisterSelfPlayer()
        {
            RegisterSelfMessage registerMsg = new RegisterSelfMessage();
            registerMsg.data.PlayerID = m_playerCount;
            m_playerCount += 1;
            m_PlayerMgr.ReciveRegisterSelfPlayer(registerMsg);
        }

        [ContextMenu("测试开始同步")]
        public void TestStartSync()
        {
            m_startedSync = true;
        }

        private void Update()
        {
            if (!m_startedSync) return;
            m_timer += Time.deltaTime;
            if (m_timer >= m_upLoadInterval)
            {
                LogicUpdate();
                m_timer = 0;
            }
        }

        private void FixedUpdate()
        {
            if (!m_startedSync) return;
            m_PlayerMgr.OnFixedUpdate(Time.fixedDeltaTime);
        }

        private void LogicUpdate()
        {
            UpLoadMessage upLoadMsg = new UpLoadMessage();
            var playerInput = upLoadMsg.data;

            playerInput.JoyX = Input.GetAxis("Horizontal");
            playerInput.JoyY = Input.GetAxis("Vertical");

            AEDebug.Log(playerInput.JoyX + "...." + playerInput.JoyY);

            playerInput.PlayerID = m_PlayerMgr.PlayerID;

            var updateMsg = new UpdateMessage();
            updateMsg.data.Delta = m_upLoadInterval;
            updateMsg.data.PlayerInputs.Add(playerInput);

            m_PlayerMgr.OnLogincUpdate(updateMsg.data);
        }
    }
}