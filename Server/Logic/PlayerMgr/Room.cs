using AE_ClientNet;
using AE_NetMessage;
using AE_ServerNet;
using NetGameRunning;

namespace LockStep_Demo_Server
{
    public class Room
    {
        public int CurFrame { get; private set; }

        private Dictionary<int, ClientSocket> m_players;

        private UpdateMessage m_currentFrameplayerInputs;
        private Dictionary<int, bool> m_IDRecived;

        private DateTime m_lastSendUpdateMsg;

        public Room()
        {
            m_currentFrameplayerInputs = new UpdateMessage();
            m_players = new Dictionary<int, ClientSocket>();
            m_IDRecived = new Dictionary<int, bool>();

            ClientSocket.AddListener(MessagePool.UpLoadMessage_ID, RecivePlayerInput);
            ClientSocket.AddListener(MessagePool.RegisterSelfMessage_ID, ReciveRegisterSelfPlayer);
            ClientSocket.AddListener(MessagePool.HeartMessage_ID, ReciveHearMessage);
            ClientSocket.AddListener(MessagePool.StartRoomMassage_ID, StartRoom);
        }

        /// <summary>
        /// 房间开始
        /// </summary>
        /// <param name="message"></param>
        /// <param name="socket"></param>
        private void StartRoom(BaseMessage message, ClientSocket socket)
        {
            CurFrame = 0;
            m_lastSendUpdateMsg = DateTime.Now;

            m_currentFrameplayerInputs.data.CurFrameIndex = CurFrame;
            m_currentFrameplayerInputs.data.NextFrameIndex = CurFrame + 1;
            foreach (var item in m_players)
            {
                var playerInput = new PlayerInputData();
                playerInput.PlayerID = item.Key;
                playerInput.JoyX = 0;
                playerInput.JoyY = 0;
                m_currentFrameplayerInputs.data.PlayerInputs.Add(playerInput);
            }

            socket.serverSocket.Broadcast(m_currentFrameplayerInputs);
            m_currentFrameplayerInputs.data.PlayerInputs.Clear();
            AEDebug.Log("接收到房间开始并发布第0帧");
        }

        /// <summary>
        /// 接收到玩家操作
        /// </summary>
        /// <param name="message"></param>
        /// <param name="socket"></param>
        private void RecivePlayerInput(BaseMessage message, ClientSocket socket)
        {
            lock (m_currentFrameplayerInputs)
            {
                var upLoadMessage = message as UpLoadMessage;
                if (upLoadMessage.data.CurFrameIndex == CurFrame + 1)
                {
                    m_IDRecived[upLoadMessage.data.PlayerID] = true;
                    m_currentFrameplayerInputs.data.PlayerInputs.Add(upLoadMessage.data);

                    AEDebug.Log("接收第" + upLoadMessage.data.CurFrameIndex + "帧" + "输入数据为" + upLoadMessage.data.JoyX +
                                "..." + upLoadMessage.data.JoyY);
                    foreach (var item in m_IDRecived.Values)
                    {
                        if (!item)
                        {
                            return;
                        }
                    }

                    //服务器帧更新
                    CurFrame += 1;
                    var span = DateTime.Now - m_lastSendUpdateMsg;
                    m_lastSendUpdateMsg = DateTime.Now;
                    AEDebug.Log(span.TotalSeconds.ToString());
                    //广播
                    m_currentFrameplayerInputs.data.CurFrameIndex = CurFrame;
                    m_currentFrameplayerInputs.data.NextFrameIndex = CurFrame + 1;
                    m_currentFrameplayerInputs.data.Delta = (float)span.TotalSeconds;
                    socket.serverSocket.Broadcast(m_currentFrameplayerInputs);
                    AEDebug.Log("发布第" + upLoadMessage.data.CurFrameIndex + "帧");
                    //清理
                    m_currentFrameplayerInputs.data.PlayerInputs.Clear();
                    for (int i = 0; i < m_IDRecived.Count; i++)
                    {
                        m_IDRecived[i] = false;
                    }
                }
            }
        }

        /// <summary>
        /// 接收注册消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="socket"></param>
        private void ReciveRegisterSelfPlayer(BaseMessage message, ClientSocket socket)
        {
            if (m_players.ContainsValue(socket))
            {
                return;
            }

            foreach (var client in socket.serverSocket.clientSockets.Values)
            {
                //返回注册当前请求客户端
                if (socket == client)
                {
                    //注册自己
                    RegisterSelfMessage registerSelfMessage = new RegisterSelfMessage();
                    registerSelfMessage.data.PlayerID = m_players.Count;
                    client.Send(registerSelfMessage);
                }
                //返回注册其他客户端
                else
                {
                    RegisterMessage registerMessage = new RegisterMessage();
                    registerMessage.data.PlayerID = m_players.Count;
                    client.Send(registerMessage);
                }
            }

            m_IDRecived.Add(m_players.Count, false);
            m_players.Add(m_players.Count, socket);
        }

        /// <summary>
        /// 接收到心跳消息
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void ReciveHearMessage(BaseMessage arg1, ClientSocket arg2)
        {
            AEDebug.Log("心跳消息");
        }
    }
}