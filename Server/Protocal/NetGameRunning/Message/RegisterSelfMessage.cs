namespace NetGameRunning{
public class RegisterSelfMessage : AE_NetMessage.BaseMessage<NetGameRunning.RegisterMessageData>{
public override int GetMessageID()
{
return 10003;
}public override void WriteIn(byte[] buffer, int beginIndex,int length)
{
 data = NetGameRunning.RegisterMessageData.Parser.ParseFrom(buffer, beginIndex, length);
}
}
}