namespace TrucoProject.Net.Messages
{
    public class ServerPingMessage : MessageBase
    {
        public ServerPingMessage() : base("ping") { }
    }
}