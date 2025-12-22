
namespace TrucoProject.Net.Messages
{
    public class ServerPongMessage : MessageBase {
        public ServerPongMessage() {
            Type = Protocol.ProtocolKeys.Pong;
        }
    }
}