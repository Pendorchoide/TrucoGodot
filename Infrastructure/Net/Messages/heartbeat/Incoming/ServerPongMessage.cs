
namespace TrucoProject.Net.Messages
{
    public class ServerPongMessage : MessageBase {
        public ServerPongMessage() {
            type = Protocol.ProtocolKeys.Pong;
        }
    }
}