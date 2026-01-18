
namespace TrucoProject.Net.Messages
{
    public class PongMessage : MessageBase {
        public PongMessage() {
            type = Protocol.ProtocolKeys.Pong;
        }
    }
}
