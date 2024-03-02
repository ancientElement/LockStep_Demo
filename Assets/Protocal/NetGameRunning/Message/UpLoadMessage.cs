namespace NetGameRunning{
public class UpLoadMessage : AE_NetMessage.BaseMessage<NetGameRunning.PlayerInputData>{
public override int GetMessageID()
{
return 10002;
}public override void WriteIn(byte[] buffer, int beginIndex,int length)
{
 data = NetGameRunning.PlayerInputData.Parser.ParseFrom(buffer, beginIndex, length);
}
}
}