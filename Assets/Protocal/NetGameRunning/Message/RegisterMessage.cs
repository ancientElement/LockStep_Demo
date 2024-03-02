namespace NetGameRunning{
public class RegisterMessage : AE_NetMessage.BaseMessage<NetGameRunning.RegisterMessageData>{
public override int GetMessageID()
{
return 10004;
}public override void WriteIn(byte[] buffer, int beginIndex,int length)
{
 data = NetGameRunning.RegisterMessageData.Parser.ParseFrom(buffer, beginIndex, length);
}
}
}