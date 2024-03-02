using AE_NetMessage;

namespace AE_ServerNet
{
    public static class MainHandler
    {
        public static void AddAllListener()
        {
          
        }

        private static void HeartMessageHandler(BaseMessage arg1, ClientSocket client)
        {
            AEDebug.Log($"接收到心跳消息:[{client.socket.RemoteEndPoint}]");
        }
    }
}
