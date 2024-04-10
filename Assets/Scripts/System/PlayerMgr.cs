using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using AE_BEPUPhysics_Addition;
using AE_ClientNet;
using AE_NetMessage;
using NetGameRunning;
using UnityEngine;

namespace LockStep_Demo
{
    public class PlayerMgr
    {
        public int PlayerID { get; private set; } //控制的玩家的ID
        private Dictionary<int, BasePlayer> m_players; //键为ID
        private AEPhysicsMgr m_physicsMgr;

        public PlayerMgr(AEPhysicsMgr physicsMgr)
        {
            m_players = new Dictionary<int, BasePlayer>();
            m_physicsMgr = physicsMgr;
            PlayerID = -1;

            NetAsyncMgr.AddNetMessageListener(MessagePool.RegisterMessage_ID, ReciveRegisterPlayer);
            NetAsyncMgr.AddNetMessageListener(MessagePool.RegisterSelfMessage_ID, ReciveRegisterSelfPlayer);
        }

        /// <summary>
        /// 逻辑更新,受到更新消息后更新
        /// </summary>
        /// <param name="msg"></param>
        public void OnLogincUpdate(UpdateMessageData updateData)
        {
            for (int i = 0; i < updateData.PlayerInputs.Count; i++)
            {
                var playerInput = updateData.PlayerInputs[i];
                var ID = playerInput.PlayerID;
                m_players[ID].OnLogicUpdate(updateData.Delta, playerInput);
            }
        }

        public void OnFixedUpdate(float delta)
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                m_players[i].OnFixedUpdate(delta);
            }
        }

        /// <summary>
        /// 发送注册当前客户端玩家
        /// </summary>
        public void SendRegisterPlayer()
        {
            RegisterSelfMessage registerSelfMsg = new RegisterSelfMessage();
            NetAsyncMgr.Send(registerSelfMsg);
        }

        /// <summary>
        /// 接收注册自己
        /// </summary>
        public void ReciveRegisterSelfPlayer(BaseMessage msg)
        {
            RegisterSelfMessage registerSelfMessage = msg as RegisterSelfMessage;
            PlayerID = registerSelfMessage.data.PlayerID;
            RegisterPlayer(registerSelfMessage.data.PlayerID);
        }

        /// <summary>
        /// 接收注册玩家
        /// </summary>
        public void ReciveRegisterPlayer(BaseMessage msg)
        {
            RegisterMessage registerMsg = msg as RegisterMessage;
            RegisterPlayer(registerMsg.data.PlayerID);
        }

        /// <summary>
        /// 真正注册玩家
        /// </summary>
        /// <param name="playerID"></param>
        /// <exception cref="NotImplementedException"></exception>
        private BasePlayer RegisterPlayer(int playerID)
        {
            var prefab = Resources.Load<GameObject>("Player");

            var go = GameObject.Instantiate(prefab);

            BasePlayer player = new BasePlayer(BasePlayer.STATEENUM.idle, go, 30f);
            m_players.Add(playerID, player);
            m_physicsMgr.RegisterCollider(go.GetComponent<BaseVolumnBaseCollider>());
            AEDebug.Log("注册玩家");
            return player;
        }
    }
}