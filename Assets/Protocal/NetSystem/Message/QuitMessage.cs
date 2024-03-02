namespace NetSystem{
public class QuitMessage : AE_NetMessage.BaseSystemMessage{
public override int GetMessageID()
{
return 1;
}
}
}