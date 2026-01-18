
namespace TrucoProject.Net.Messages
{
    public class ServerPingMessage : MessageBase {
        public ServerPingMessage() {
            type = Protocol.ProtocolKeys.Ping;
        }
    }
}
