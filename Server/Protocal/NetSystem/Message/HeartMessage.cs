namespace NetSystem{
public class HeartMessage : AE_NetMessage.BaseSystemMessage{
public override int GetMessageID()
{
return 2;
}
}
}