
namespace TrucoProject.Net.Messages
{
    public class ServerPingMessage : MessageBase {
        public ServerPingMessage() {
            Type = Protocol.ProtocolKeys.Ping;
        }
    }
}
