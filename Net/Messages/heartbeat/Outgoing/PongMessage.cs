
namespace TrucoProject.Net.Messages
{
    public class PongMessage : MessageBase {
        public PongMessage() {
            Type = Protocol.ProtocolKeys.Pong;
        }
    }
}
