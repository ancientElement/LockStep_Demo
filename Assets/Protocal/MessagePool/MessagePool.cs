namespace AE_ClientNet
{
public static class MessagePool
{
public static int QuitMessage_ID = 1;
public static int HeartMessage_ID = 2;
public static int UpdateMessage_ID = 10001;
public static int UpLoadMessage_ID = 10002;
public static int RegisterSelfMessage_ID = 10003;
public static int RegisterMessage_ID = 10004;
public static int StartRoomMassage_ID = 10005;
static int[] messageIDs = new int[] {1,2,10001,10002,10003,10004,10005};
public static int[] MessageIDs => messageIDs;
 private static readonly System.Collections.Generic.Dictionary<int, System.Func<AE_NetMessage.BaseMessage>> MessageTypeMap = new System.Collections.Generic.Dictionary<int, System.Func<AE_NetMessage.BaseMessage>>
        {
{1,() => new NetSystem.QuitMessage()},
{2,() => new NetSystem.HeartMessage()},
{10001,() => new NetGameRunning.UpdateMessage()},
{10002,() => new NetGameRunning.UpLoadMessage()},
{10003,() => new NetGameRunning.RegisterSelfMessage()},
{10004,() => new NetGameRunning.RegisterMessage()},
{10005,() => new NetGameRunning.StartRoomMassage()}
};
public static AE_NetMessage.BaseMessage GetMessage(int id) {       if (MessageTypeMap.TryGetValue(id, out System.Func<AE_NetMessage.BaseMessage> messageFactory)) {                     return messageFactory?.Invoke();        }        return null;   }
}
}
