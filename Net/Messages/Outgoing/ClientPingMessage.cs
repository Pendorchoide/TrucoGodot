
namespace TrucoProject.Net.Messages
{
    public class ClientPingMessage : MessageBase
    {
        public ClientPingMessage()
        {
            Type = "ping";
        }
    }
}
