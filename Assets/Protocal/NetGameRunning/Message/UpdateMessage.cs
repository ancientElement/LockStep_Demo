namespace NetGameRunning{
public class UpdateMessage : AE_NetMessage.BaseMessage<NetGameRunning.UpdateMessageData>{
public override int GetMessageID()
{
return 10001;
}public override void WriteIn(byte[] buffer, int beginIndex,int length)
{
 data = NetGameRunning.UpdateMessageData.Parser.ParseFrom(buffer, beginIndex, length);
}
}
}